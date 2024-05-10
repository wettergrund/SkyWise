using Microsoft.EntityFrameworkCore;
using WeatherHandler.Data;
using WeatherHandler.Models;

namespace WeatherHandler.Repositories;

public class MetarRepo(WxDbContext context) : RepoBase<METAR>(context), IMetarRepo
{
    public async Task<METAR?> GetMetarByIcaoAsync(string icao)
    {
        var response = await _db.METAR.Where(x => x.ICAO == icao).FirstOrDefaultAsync();

        return response;
    }

    public bool RemoveOldMetars()
    {
        var timeLimit = DateTime.Today.AddDays(-1);

        var toBeRemoved = _db.METAR.Where(m => m.ValidFrom < timeLimit).ToList();

        _db.RemoveRange(toBeRemoved);

        return true;

    }
}