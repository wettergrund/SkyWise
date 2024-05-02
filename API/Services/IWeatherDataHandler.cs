using API.Models;
using API.Models.DB;

namespace API.Services
{
    public interface IWeatherDataHandler
    {

        public Task<AirportWeather> GetWeatherByICAO(string ICAO);



        public Task<bool> FetchMetar();
        public Task<bool> FetchTaf();

        public Task<List<WeatherResponse>> GetWxByLine(string from, string to);
        



    }
}
