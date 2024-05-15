using System.IO.Compression;
using NetTopologySuite.Geometries;
using WeatherHandler.Models;
using WeatherHandler.Repositories;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace WeatherHandler.Services;

public class WxServices(IAirportRepo apRepo, IMetarRepo metarRepo, IRepoBase<TAF> tafRepo, IRedisHandler redisRepo, IConfiguration config) : IWxServices
{
    private string _awUrl = "https://aviationweather.gov/data/cache/";

    public async Task<bool> FetchMetar()
    {
        string sourceFile = _awUrl + "metars.cache.csv.gz";

        using (HttpClient client = new HttpClient())
        await using (Stream stream = await client.GetStreamAsync(sourceFile))
        await using (GZipStream zip = new GZipStream(stream, CompressionMode.Decompress))
        using (StreamReader reader = new StreamReader(zip))
        {
            int startLine = 6;
            string limit = config["limiter"] ?? "";

            for (int i = 0; i < startLine; i++)
            {
                if (!reader.EndOfStream)
                {
                    await reader.ReadLineAsync();
                }
            }

            while (!reader.EndOfStream)
            {
                string? csvLine = await reader.ReadLineAsync();
                if (csvLine is null)
                {
                    throw new Exception("Invalid CSV");
                }
                string[] csvColumns = csvLine.Split(',');
                string icaoCode = csvColumns[1];

                if (!icaoCode.StartsWith(limit))
                {
                    continue;
                }

                Airport? getAirport = await GetAirportFromDb(icaoCode);

                if (getAirport is null)
                {
                    string latitude = csvColumns[3];
                    string longitude = csvColumns[4];

                    Console.WriteLine(icaoCode);
                   

                    getAirport = await AddAirport(icaoCode, latitude, longitude);
                }

                var newMetar = new METAR();

                MapCsvToMetar(newMetar, getAirport, csvColumns);

                bool test = await redisRepo.UpdateRedis(newMetar);
                await metarRepo.Add(newMetar);

            }

            await metarRepo.SaveChanges();
        }

        return true;
    }

    public async Task<bool> FetchTaf()
    {
        string sourceFile = _awUrl + "tafs.cache.csv.gz";
        using (HttpClient client = new HttpClient())
        await using (Stream stream = await client.GetStreamAsync(sourceFile))
        await using (GZipStream zip = new GZipStream(stream, CompressionMode.Decompress))
        using (StreamReader reader = new StreamReader(zip))
        {
            int startLine = 6;

            for (int i = 0; i < startLine; i++)
            {
                if (!reader.EndOfStream)
                {
                    await reader.ReadLineAsync();
                }
            }
            while (!reader.EndOfStream)
            {
                string? csvLine = await reader.ReadLineAsync();

                string[] csvColumns = csvLine.Split(',');

                string raw = csvColumns[0];
                bool isCavok = raw.Contains("CAVOK");
                string icaoCode = csvColumns[1];
                string latitude = csvColumns[7];
                string longitude = csvColumns[8];

                if (!icaoCode.StartsWith("ES"))
                {
                    continue;
                }



                var getAirport = await apRepo.GetAirportByICAOAsync(icaoCode);

                if (getAirport is null)
                {
                    getAirport = await AddAirport(icaoCode, latitude, longitude);
                }


                var newTAF = new TAF()
                {
                    RawTAF = raw,
                    ICAO = icaoCode,
                    IssueTime = DateTime.Parse(csvColumns[2]).ToUniversalTime(),
                    ValidFrom = DateTime.Parse(csvColumns[4]).ToUniversalTime(),
                    ValidTo = DateTime.Parse(csvColumns[5]).ToUniversalTime(),
                    Remarks = csvColumns[6],
                    Forcasts = ParseForcastsFromCsv(csvColumns),
                    Airport = getAirport,
                };

                bool redis = await redisRepo.UpdateRedis(newTAF);
                await tafRepo.Add(newTAF);
            }

            await tafRepo.SaveChanges();

        }

        return true;
    }

    public async Task<bool> CleanUp()
    {
        return metarRepo.RemoveOldMetars();
    }


    private void MapCsvToMetar(METAR metarObj, Airport airport, string[] csvColumns)
    {
        string rawMetar = csvColumns[0];
        bool isCavok = rawMetar.Contains("CAVOK");
        string icaoCode = airport.ICAO;
        double temp = string.IsNullOrEmpty(csvColumns[5])
            ? 0
            : double.Parse(csvColumns[5]);

        double dewPoint = string.IsNullOrEmpty(csvColumns[6])
            ? 0
            : double.Parse(csvColumns[6]);

        double? qnh = TryExtractQnh(rawMetar);


        metarObj.RawMetar = rawMetar;
        metarObj.ICAO = icaoCode;
        metarObj.ValidFrom = DateTime.TryParse(csvColumns[2], out DateTime validFrom)
             ? validFrom.ToUniversalTime()
             : DateTime.MinValue;
        metarObj.Temp = (int)Math.Round(temp);
        metarObj.DewPoint = (int)Math.Round(dewPoint);
        metarObj.WindDirectionDeg = csvColumns[7] == "VRB"
            ? -1
            : Convert.ToInt32(csvColumns[7]);
        metarObj.WindSpeedKt = Convert.ToInt32(csvColumns[8]);
        metarObj.WindGustKt = string.IsNullOrEmpty(csvColumns[9])
            ? 0
            : Convert.ToInt32(csvColumns[9]);
        metarObj.VisibilityM = ExtractVisibility(rawMetar);
        metarObj.QNH = qnh
            ?? (!string.IsNullOrEmpty(csvColumns[12])
                    ? double.Parse(csvColumns[12])
                    : -1);
        metarObj.VerticalVisibilityFt = string.IsNullOrEmpty(csvColumns[41])
            ? null
            : int.Parse(csvColumns[41]);
        metarObj.WxString = csvColumns[21];
        metarObj.CloudLayers = ParseCloudInfo(csvColumns);
        metarObj.Rules = csvColumns[30];
        metarObj.Airport = airport;
        metarObj.Auto = !string.IsNullOrEmpty(csvColumns[14]);


    }

    private List<Forcast> ParseForcastsFromCsv(string[] csvColumn)
    {
        var forcastList = new List<Forcast>();

        int forecastStart = 10;
        int lastForecastStart = 269;
        int columnsInEachForecast = 37;

        for (int i = forecastStart;
                i <= lastForecastStart;
                i += columnsInEachForecast)
        {
            if (string.IsNullOrEmpty(csvColumn[i]))
            {
                break;
            }

            var newForecast = new Forcast();

            MapForecast(newForecast, csvColumn, i);

            forcastList.Add(newForecast);
        }

        return forcastList;
    }
    private void MapForecast(Forcast model, string[] csvColumn, int i)
    {
        var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        model.ForcastFromTime = DateTime.Parse(csvColumn[i]).ToUniversalTime();
        model.ForcastToTime = DateTime.Parse(csvColumn[i + 1]).ToUniversalTime();
        model.ChangeIndicator = csvColumn[i + 2];
        model.BecomingTime = string.IsNullOrEmpty(csvColumn[i + 3])
            ? DateTime.MinValue
            : unixEpoch.AddSeconds(double.Parse(csvColumn[i + 3])).ToUniversalTime(); //FIX?
        model.WindDirectionDeg = csvColumn[i + 5] == "VRB" || string.IsNullOrEmpty(csvColumn[i + 5])
            ? 0
            : int.Parse(csvColumn[i + 5]);
        model.WindSpeedKt = string.IsNullOrEmpty(csvColumn[i + 6])
            ? 0
            : int.Parse(csvColumn[i + 6]);
        model.WindGustKt = string.IsNullOrEmpty(csvColumn[i + 7])
            ? 0
            : int.Parse(csvColumn[i + 7]);
        model.VisibilityM = ConvertMileToMeter(csvColumn[i + 11]) ?? -1; //FIX
        model.VerticalVisibilityFt = string.IsNullOrEmpty(csvColumn[i + 13])
            ? null
            : int.Parse(csvColumn[i + 13]);
        model.WxString = csvColumn[i + 14];
        model.CloudLayers = ParseCloudInfo(csvColumn, i + 16, i + 22, 3);
        model.Probability = ParseProb(csvColumn[i + 4]);
    }
    private static Probability ParseProb(string probSting)
    {
        if (string.IsNullOrEmpty(probSting)) return Probability.Empty;

        return probSting == "40" ? Probability.P40 : Probability.P30;
    }



    private async Task<Airport?> GetAirportFromDb(string icao)
    {

        var getAirport = await apRepo.GetAirportByICAOAsync(icao);

        if (getAirport is null) return null;

        return getAirport;

    }

    private static Point GeneratePointFromString(string latitude, string longitude)
    {
        double lat = Convert.ToDouble(latitude);
        double lon = Convert.ToDouble(longitude);

        var coordinate = new Coordinate()
        {
            Y = lat,
            X = lon,
        };

        return new Point(coordinate) { SRID = 4326 };

    }
    private List<CloudModel> ParseCloudInfo(string[] rawData, int startPos = 22, int endPos = 26, int inc = 2)
    {
        var list = new List<CloudModel>();
        for (int i = startPos; i <= endPos; i += inc)
        {
            if (string.IsNullOrEmpty(rawData[i]))
            {
                continue;
            }

            int cloudBase;
            bool success = int.TryParse(rawData[i + 1], out cloudBase);

            var newCloudLayer = new CloudModel()
            {
                Cover = rawData[i],
                CloudBase = success ? cloudBase : 0,
                CloudType = inc == 3 ? rawData[i + 2] : ""
            };
            list.Add(newCloudLayer);
        }

        return list;
    }

    private int ExtractVisibility(string rawString)
    {
        Regex visibilityRegex = new Regex(@"\b\d{4}\b");

        Match match = visibilityRegex.Match(rawString);

        if (match.Success)
        {
            return int.Parse(match.Value);
        }

        return -1;
    }

    private double? TryExtractQnh(string rawMetar)
    {

        Regex qnhRegex = new Regex(@"Q(\d{4})");

        Match qnhResult = qnhRegex.Match(rawMetar);
        if (qnhResult.Success)
        {
            // Extract the captured digits (group 1) and convert to double
            if (double.TryParse(qnhResult.Groups[1].Value, out double qnhValue))
            {
                return qnhValue;
            }
        }
 

        return null;

    }

    private int? ConvertMileToMeter(string mile)
    {

        if (string.IsNullOrEmpty(mile)) return null;
        string pattern = @"\+";

        string removedPlus = Regex.Replace(mile, pattern, "");

        double mileDouble;

        if (!double.TryParse(removedPlus, out mileDouble))
        {
            throw new Exception("Unable to convert string " + removedPlus + " to double");
        }

        double milesToMeters = mileDouble * 1609.34;

        double movedDecimal = milesToMeters / 1000;

        double roundedDecimal = Math.Round(movedDecimal, 0);

        int result = (int)roundedDecimal * 1000;

        return result;
    }
    private async Task<Airport> AddAirport(string ICAO, string latitude, string longitude)
    {
        var newAirport = new Airport()
        {
            ICAO = ICAO,
            Location = GeneratePointFromString(latitude, longitude)
        };

        await apRepo.Add(newAirport);
        await apRepo.SaveChanges();

        return newAirport;
    }

}
