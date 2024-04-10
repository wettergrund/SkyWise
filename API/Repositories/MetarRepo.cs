using API.Data;
using API.Models.DB;

namespace API.Repositories
{
    public class MetarRepo : RepoBase<METAR>, IMetarRepo
    {
        public MetarRepo(SWContext context) : base(context) {}
        public async Task<METAR> GetMetarAsync(string icao)
        {
            return _db.METAR.Where(m => m.ICAO == icao).First();
        }
    }
}
