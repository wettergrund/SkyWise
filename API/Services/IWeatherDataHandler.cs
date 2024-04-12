using API.Models;
using API.Models.DB;

namespace API.Services
{
    public interface IWeatherDataHandler
  {

        public Task<AirportWeather> GetWeatherByICAO(string ICAO);

        public Task<bool> AddTaf();


        public Task FetchMetar();
        public Task FetchTaf();

        


  }   
}
