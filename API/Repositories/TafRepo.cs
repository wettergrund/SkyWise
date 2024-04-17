using API.Data;
using API.Models.DB;

namespace API.Repositories
{
    public class TafRepo : RepoBase<TAF>, ITafRepo
    {
        public TafRepo(SWContext context) : base(context) { }

        public async Task<TAF> GetTafAsync(string icao)
        {
            var result = _db.TAF.Where(m => m.ICAO == icao).FirstOrDefault();
            return result;

        }
    }
}
