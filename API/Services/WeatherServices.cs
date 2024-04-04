namespace API.Services
{
  public interface IWeatherDataHandler
  {

    public Task<string> GetWeatherByICAO(string ICAO);
      
  }   

  class WeatherDataHandler : IWeatherDataHandler
  {
    public async Task<string> GetWeatherByICAO(string ICAO){

      return ICAO;

    }    
  }
}
