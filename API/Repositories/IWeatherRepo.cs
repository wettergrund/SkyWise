using API.Models;

namespace API.Repositories
{
    public interface IWeatherRepo
    {
        Task<AirportWeather> GetAirportWeatherAsync(string icao);

    }


}
