using Microsoft.EntityFrameworkCore;
using WeatherHandler.Data;
using WeatherHandler.Models;

namespace WeatherHandler.Repositories;

public class AirportRepo(WxDbContext context) : RepoBase<Airport>(context), IAirportRepo
{
    public async Task<Airport?> GetAirportByICAOAsync(string icao)
    {
        var response =  await _db.Airport.Where(ap => ap.ICAO == icao).FirstOrDefaultAsync();
        return response;
        
        
    }
}