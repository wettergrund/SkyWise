using API.Models.DB;

namespace API.Repositories
{
    public interface ITafRepo : IRepoBase<TAF>
    {
        Task<TAF> GetTafAsync(string icao);
    }
}
