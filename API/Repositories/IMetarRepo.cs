using API.Models.DB;
using API.Models.DTO;

namespace API.Repositories
{
    public interface IMetarRepo : IRepoBase<METAR>
    {
        Task<MetarResponse> GetMetarAsync(string icao);
    }
}
