using System.IO.Compression;
using API.Data;
using API.Models;
using API.Models.DB;
using API.Repositories;
using Microsoft.IdentityModel.Tokens;

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

            if (ap == null) {
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

        public async Task<AirportWeather> GetWeatherByICAO(string ICAO){

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
        
        using(HttpClient client = new HttpClient())
            await using(Stream stream = await client.GetStreamAsync(sourceFile))
                await using (GZipStream zip = new GZipStream(stream, CompressionMode.Decompress))
                using (StreamReader reader = new StreamReader(zip))
                {
                    int startLine = 6;
                    int limit = 2;

                    for (int i = 0; i < startLine; i++)
                    {
                        if (!reader.EndOfStream)
                        {
                            await reader.ReadLineAsync();
                        }
                    }

                    int count = 0;

                    while (!reader.EndOfStream && count <= limit)
                    {
                        count++;
                        
                        string? csvLine = await reader.ReadLineAsync();

                        string[] csvColumns = csvLine.Split(',');

                        var getAirport = await _apRepo.GetAirportByICAOAsync(csvColumns[1]);

                        if (getAirport is null)
                        {
                            //TODO: Add aiport
                            getAirport = await _apRepo.GetAirportByICAOAsync("ESOW");
                        }
                        
                        
                        // map csvColumns to a METAR object.
                        // var newMetar = new METAR()
                        // {
                        //     RawMetar = csvColumns[0],
                        //     ICAO = csvColumns[1],
                        //     ValidFrom = DateTime.Parse(csvColumns[2]),
                        //     Temp = Convert.ToInt32(csvColumns[5]),
                        //     DewPoint = Convert.ToInt32(csvColumns[6]),
                        //     /* TODO:
                        //      * Plocka sikt manuellt från RawMetar (eg. 9999)
                        //      */
                        //     WindDirectionDeg = csvColumns[7] == "VRB" ? -1 : Convert.ToInt32(csvColumns[7]), //TODO: Could be "VRB"
                        //     WindSpeedKt = Convert.ToInt32(csvColumns[8]),
                        //     WindGustKt = csvColumns[9].IsNullOrEmpty() ? 0 : Convert.ToInt32(csvColumns[9]),
                        //     VisibilityM = 1337, //TODO
                        //     QNH = 0, //Convert.ToDouble(csvColumns[11]), //TODO - Could be empty, 12 has hpa sometimes, double?
                        //     VerticalVisibilityFt = Convert.ToInt32(csvColumns[41]), // Todo: Remove for METAR?
                        //     WxString = csvColumns[21],
                        //     CloudLayers = new(),
                        //     Rules = csvColumns[30],
                        //     Airport = getAirport
                        //     
                        //     
                        // };

                        var newMetar = new METAR();
                        newMetar.RawMetar = csvColumns[0] ?? "";
                        newMetar.ICAO = csvColumns[1] ?? "";
                        newMetar.ValidFrom = DateTime.TryParse(csvColumns[2], out DateTime validFrom) ? validFrom : DateTime.MinValue;
                        newMetar.Temp = string.IsNullOrEmpty(csvColumns[5]) ? 0 : Convert.ToInt32(csvColumns[5]);
                        newMetar.DewPoint = string.IsNullOrEmpty(csvColumns[6]) ? 0 : Convert.ToInt32(csvColumns[6]);
                        newMetar.WindDirectionDeg = csvColumns[7] == "VRB" ? -1 : Convert.ToInt32(csvColumns[7]);
                        newMetar.WindSpeedKt = Convert.ToInt32(csvColumns[8]);
                        newMetar.WindGustKt = string.IsNullOrEmpty(csvColumns[9]) ? 0 : Convert.ToInt32(csvColumns[9]);
                        newMetar.VisibilityM = 1337; // Default value changed to 1337
                        newMetar.QNH = string.IsNullOrEmpty(csvColumns[11]) ? 0.0 : Convert.ToDouble(csvColumns[11]); // Default value changed to 0.0
                        newMetar.VerticalVisibilityFt = 0; // Todo: Remove for METAR?
                        newMetar.WxString = csvColumns[21];
                        newMetar.CloudLayers = new();
                        newMetar.Rules = csvColumns[30];
                        newMetar.Airport = getAirport;

                      
                        //Handle cloudLayers
                        for (int i = 22; i <= 26; i += 2)
                        {
                            int cloudBase;
                            bool success = int.TryParse(csvColumns[i + 1], out cloudBase);
                            
                            var newCloudLayer = new CloudModel()
                            {
                                Cover = csvColumns[i],
                                CloudBase = success ? cloudBase : 0,
                                CloudType = ""
                            };
                            newMetar.CloudLayers.Add(newCloudLayer);
                        }
                        
                        
                        
                        // TODO: Add to DB
                        _metarRepo.Add(newMetar);
                        
                    }
                        _metarRepo.SaveChanges();

                    
                }

        return true;
    }

    public async Task<bool> FetchTaf()
    {
        throw new NotImplementedException();
    }
    
   

  }
}
