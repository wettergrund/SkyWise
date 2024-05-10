using WeatherHandler.Models;

namespace WeatherHandler.Repositories;

public interface IAirportRepo : IRepoBase<Airport>
{
    Task<Airport?> GetAirportByICAOAsync(string icao);
    

}