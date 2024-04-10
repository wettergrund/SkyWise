using API.Data;
using API.Models;
using API.Models.DB;

namespace API.Repositories
{
    public class WeatherRepo : IWeatherRepo
    {
        private readonly IMetarRepo _metarRepo;
        private readonly ITafRepo _tafRepo;
        public WeatherRepo(IMetarRepo metarRepo, ITafRepo tafRepo)
        {
            _metarRepo = metarRepo;
            _tafRepo = tafRepo;

        }

        public async Task<AirportWeather> GetAirportWeatherAsync(string icao)
        {

            var metar = await _metarRepo.GetMetarAsync(icao);
            var taf = await _tafRepo.GetTafAsync(icao);

            return new AirportWeather { Metar = metar, Taf = taf};

        }
    }
}
