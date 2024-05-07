using WeatherHandler.Data;
using WeatherHandler.Models;

namespace WeatherHandler.Repositories;

public class AirportRepo(WxDbContext context) : RepoBase<Airport>(context), IAirportRepo
{
    public Task<Airport> GetAirportByICAOAsync(string icao)
    {
        throw new NotImplementedException();
    }
}