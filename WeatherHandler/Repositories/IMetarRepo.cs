using WeatherHandler.Models;

namespace WeatherHandler.Repositories;

public interface IMetarRepo : IRepoBase<METAR>
{

    Task<METAR?> GetMetarByIcaoAsync(string icao);

    public bool RemoveOldMetars();

}