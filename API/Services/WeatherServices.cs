using API.Data;
using API.Models;
using API.Models.DB;
using API.Repositories;

namespace API.Services
{

    class WeatherDataHandler : IWeatherDataHandler
  {
        private readonly IWeatherRepo _repo;
        private readonly ITafRepo _tafRepo;
        private readonly IRepoBase<Airport> _apRepo;
        private readonly IMetarRepo _metarRepo;

        public WeatherDataHandler(IWeatherRepo repo, ITafRepo tafRepo, IRepoBase<Airport> apRepo, IMetarRepo metarRepo)
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
  }
}
