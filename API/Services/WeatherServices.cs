using API.Data;
using API.Models;
using API.Models.DB;
using API.Repositories;

namespace API.Services
{
  public interface IWeatherDataHandler
  {

    public Task<AirportWeather> GetWeatherByICAO(string ICAO);

      
  }   

  class WeatherDataHandler : IWeatherDataHandler
  {
        private readonly IWeatherRepo _repo;
        public WeatherDataHandler(IWeatherRepo repo)
        {
            _repo = repo;
            
        }
        public async Task<AirportWeather> GetWeatherByICAO(string ICAO){

            var result = await _repo.GetAirportWeatherAsync(ICAO);

            

            return result;

    }    
  }
}
