using API.Data;
using API.Models.DB;
using API.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class TafRepo : RepoBase<TAF>, ITafRepo
    {
        public TafRepo(SWContext context) : base(context) { }

        public async Task<TafResponse?> GetTafAsync(string icao)
        {
            var dbResult = await _db.TAF.Where(m => m.ICAO == icao).FirstOrDefaultAsync();

            if (dbResult is null)
            {
                return null;
            }

            var newTafResponse = new TafResponse(dbResult);
            
            return newTafResponse;

        }
    }
}
