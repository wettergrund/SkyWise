using API.Models.DB;
using API.Models.DTO;

namespace API.Repositories
{
    public interface ITafRepo : IRepoBase<TAF>
    {
        Task<TafResponse?> GetTafAsync(string icao);
    }
}
