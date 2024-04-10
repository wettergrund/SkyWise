using API.Models.DB;

namespace API.Repositories
{
    public interface IMetarRepo : IRepoBase<METAR>
    {
        Task<METAR> GetMetarAsync(string icao);
    }
}
