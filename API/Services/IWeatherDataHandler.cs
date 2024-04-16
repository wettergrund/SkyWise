using API.Models;
using API.Models.DB;

namespace API.Services
{
    public interface IWeatherDataHandler
  {

        public Task<AirportWeather> GetWeatherByICAO(string ICAO);

        public Task<bool> AddTaf();


        public Task<bool> FetchMetar();
        public Task<bool> FetchTaf();

        


  }   
}
