using API.Models;
using API.Models.DB;
using API.Repositories;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite.Geometries;
using System.IO.Compression;
using System.Security.Claims;
using System.Text.RegularExpressions;


namespace API.Services
{
    class WeatherDataHandler : IWeatherDataHandler
    {
        private readonly IWeatherRepo _repo;
        private readonly ITafRepo _tafRepo;
        private readonly IAirportRepo _apRepo;
        private readonly IMetarRepo _metarRepo;

        public WeatherDataHandler(IWeatherRepo repo, ITafRepo tafRepo, IAirportRepo apRepo, IMetarRepo metarRepo)
        {
            _repo = repo;
            _tafRepo = tafRepo;
            _apRepo = apRepo;
            _metarRepo = metarRepo;
        }

        public async Task<bool> AddTaf()
        {
            var ap = _apRepo.GetAll().Where(airport => airport.ICAO == "ESOW").FirstOrDefault();

            if (ap == null)
            {
                return false;
            }

            var newMetar = new METAR()
            {
                ICAO = ap.ICAO,
                RawMetar = "ESOW 111350Z 21012KT 190V250 9999 BKN013 10/07 Q1010",
                Temp = 10,
                DewPoint = 7,
                WindDirectionDeg = 210,
                WindSpeedKt = 12,
                VisibilityM = 9999,
                VerticalVisibilityFt = 0,
                QNH = 1010,
                WxString = "",
                Auto = false,
                CloudLayers = new()
                {
                    new()
                    {
                        CloudBase = 1300,
                        Cover = "BKN"
                    }
                },
                Rules = "IFR",
                Airport = ap
            };

            var newTaf = new TAF()
            {
                ICAO = ap.ICAO,
                RawTAF = "TAF ESOW 120530Z 1206/1215 23006KT CAVOK PROB40 1214/1215 BKN012",
                IssueTime = DateTime.Now,
                ValidFrom = DateTime.Now,
                ValidTo = DateTime.Now.AddHours(12),
                Airport = ap
            };

            List<Forcast> forcast = new List<Forcast>()
            {
                new()
                {
                    ForcastFromTime = DateTime.Now,
                    ForcastToTime = DateTime.Now.AddHours(12),
                    ChangeIndicator = "",
                    BecomingTime = DateTime.Now,
                    Probability = Probability.Empty,
                    WindDirectionDeg = 180,
                    WindSpeedKt = 10,
                    WindGustKt = 15,
                    VisibilityM = 9999,
                    VerticalVisibilityFt = 1000,
                    WxString = "-SN",
                    CloudLayers = new()
                }
            };

            var newCloudlayer = new CloudModel()
            {
                CloudBase = 1500,
                CloudType = "",
                Cover = "BKN"
            };

            forcast.First().CloudLayers.Add(newCloudlayer);

            newTaf.Forcasts = forcast;

            await _metarRepo.Add(newMetar);
            await _tafRepo.Add(newTaf);
            await _tafRepo.SaveChanges();
            await _metarRepo.SaveChanges();


            return true;
        }

        public async Task<AirportWeather> GetWeatherByICAO(string ICAO)
        {

            var result = await _repo.GetAirportWeatherAsync(ICAO);


            return result;
        }

        public async Task<bool> FetchMetar()
        {
            /*
             * 1. Get CSV
             * 2. Turn each line into an object of METAR
             * 3. Add each object to DB
             */

            string sourceFile = "https://aviationweather.gov/data/cache/metars.cache.csv.gz";

            //var aiports = _apRepo.GetAll();

            using (HttpClient client = new HttpClient())
            await using (Stream stream = await client.GetStreamAsync(sourceFile))
            await using (GZipStream zip = new GZipStream(stream, CompressionMode.Decompress))
            using (StreamReader reader = new StreamReader(zip))
            {
                int startLine = 6;
                int limit = 0;

                for (int i = 0; i < startLine; i++)
                {
                    if (!reader.EndOfStream)
                    {
                        await reader.ReadLineAsync();
                    }
                }

                int count = 0;

                while (!reader.EndOfStream)
                {
                    string? csvLine = await reader.ReadLineAsync();

                    string[] csvColumns = csvLine.Split(',');

                    string raw = csvColumns[0];
                    bool isCavok = raw.Contains("CAVOK");
                    string icaoCode = csvColumns[1];
                    string latitude = csvColumns[3];
                    string longitude = csvColumns[4];

                    if (!icaoCode.StartsWith("ES"))
                    {
                        continue;
                    }

                    count++;


                    double temp = string.IsNullOrEmpty(csvColumns[5])
                        ? 0
                        : double.Parse(csvColumns[5]);

                    double dewPoint = string.IsNullOrEmpty(csvColumns[6])
                        ? 0
                        : double.Parse(csvColumns[6]);

                    var getAirport = await _apRepo.GetAirportByICAOAsync(icaoCode);

                    if (getAirport is null)
                    {
                        //TODO: Add aiport
                        getAirport = await AddAiport(icaoCode, latitude, longitude);
                    }


                    var newMetar = new METAR();

                    newMetar.RawMetar = raw;
                    newMetar.ICAO = csvColumns[1] ?? "";
                    newMetar.ValidFrom = DateTime.TryParse(csvColumns[2], out DateTime validFrom)
                        ? validFrom.ToUniversalTime()
                        : DateTime.MinValue;
                    newMetar.Temp = (int)Math.Round(temp);
                    newMetar.DewPoint = (int)Math.Round(dewPoint);
                    newMetar.WindDirectionDeg = csvColumns[7] == "VRB" ? -1 : Convert.ToInt32(csvColumns[7]);
                    newMetar.WindSpeedKt = Convert.ToInt32(csvColumns[8]);
                    newMetar.WindGustKt = string.IsNullOrEmpty(csvColumns[9]) ? 0 : Convert.ToInt32(csvColumns[9]);
                    newMetar.VisibilityM = ExtractVisibility(raw); // Default value changed to 1337
                    newMetar.QNH =
                        string.IsNullOrEmpty(csvColumns[11])
                            ? 0.0
                            : Convert.ToDouble(csvColumns[11]); // Default value changed to 0.0
                    newMetar.VerticalVisibilityFt = 0; // Todo: Remove for METAR?
                    newMetar.WxString = csvColumns[21];
                    newMetar.CloudLayers = ParseCloudInfo(csvColumns);
                    newMetar.Rules = csvColumns[30];
                    newMetar.Airport = getAirport;


                    // TODO: Add to DB
                    _metarRepo.Add(newMetar);
                }

                await Console.Out.WriteLineAsync($"Total iterated: {count}");

                await _metarRepo.SaveChanges();
            }

            return true;
        }

        public async Task<bool> FetchTaf()
        {
            string sourceFile = "https://aviationweather.gov/data/cache/tafs.cache.csv.gz";

            using (HttpClient client = new HttpClient())
            await using (Stream stream = await client.GetStreamAsync(sourceFile))
            await using (GZipStream zip = new GZipStream(stream, CompressionMode.Decompress))
            using (StreamReader reader = new StreamReader(zip))
            {
                int startLine = 6;
                int limit = 0;

                for (int i = 0; i < startLine; i++)
                {
                    if (!reader.EndOfStream)
                    {
                        await reader.ReadLineAsync();
                    }
                }

                int count = 0;

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

                    count++;


                    var getAirport = await _apRepo.GetAirportByICAOAsync(icaoCode);

                    if (getAirport is null)
                    {
                        //TODO: Add aiport
                        getAirport = await AddAiport(icaoCode, latitude, longitude);
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

                    await _tafRepo.Add(newTAF);
                }

                await Console.Out.WriteLineAsync($"Total iterated TAF: {count}");

                await _metarRepo.SaveChanges();
            }

            return true;
        }


        private List<CloudModel> ParseCloudInfo(string[] rawData, int startPos = 22, int endPos = 26, int inc = 2)
        {
            var list = new List<CloudModel>();
            for (int i = startPos; i <= endPos; i += inc)
            {
                if (rawData[i].IsNullOrEmpty())
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

        private async Task<Airport> AddAiport(string ICAO, string latitude, string logitude)
        {
            double lat = Convert.ToDouble(latitude);
            double lon = Convert.ToDouble(logitude);

            var coordinate = new Coordinate()
            {
                Y = lat,
                X = lon,
            };


            var newAirport = new Airport()
            {
                ICAO = ICAO,
                Location = new Point(coordinate) { SRID = 4326 }
            };

            await _apRepo.Add(newAirport);
            await _apRepo.SaveChanges();

            return newAirport;
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

        private List<Forcast> ParseForcastsFromCsv(string[] csvColumn)
        {
            var forcastList = new List<Forcast>();

            for (int i = 10; i <= 269; i += 37)
            {
                if (csvColumn[i].IsNullOrEmpty())
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
                model.BecomingTime = csvColumn[i + 3].IsNullOrEmpty()
                    ? DateTime.MinValue
                    : unixEpoch.AddSeconds(double.Parse(csvColumn[i + 3])).ToUniversalTime(); //FIX?
                model.WindDirectionDeg = csvColumn[i + 5] == "VRB" || csvColumn[i + 5].IsNullOrEmpty()
                    ? 0
                    : int.Parse(csvColumn[i + 5]);
                model.WindSpeedKt = csvColumn[i + 6].IsNullOrEmpty() ? 0 : int.Parse(csvColumn[i + 6]);
                model.WindGustKt = csvColumn[i + 7].IsNullOrEmpty() ? 0 : int.Parse(csvColumn[i + 7]);
                model.VisibilityM = 1337; //FIX
                model.VerticalVisibilityFt = 1337; // FIX
                model.WxString = csvColumn[i + 14];
                model.CloudLayers = ParseCloudInfo(csvColumn, i + 16, i + 22, 3);
                model.Probability = ParseProb(csvColumn[i + 4]);

        }

        private static Probability ParseProb(string probSting)
        {
            if (probSting.IsNullOrEmpty()) return Probability.Empty;

            return probSting == "40" ? Probability.P40 : Probability.P30;

        }
    }
}