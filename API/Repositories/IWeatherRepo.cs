using API.Models;
using API.Models.DB;

namespace API.Repositories
{
    public interface IWeatherRepo
    {
        Task<AirportWeather> GetAirportWeatherAsync(string icao);

    }


}
