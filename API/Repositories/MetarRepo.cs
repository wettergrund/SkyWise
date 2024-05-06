using API.Data;
using API.Models.DB;
using API.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class MetarRepo : RepoBase<METAR>, IMetarRepo
    {
        public MetarRepo(SWContext context) : base(context) { }
        public async Task<MetarResponse> GetMetarAsync(string icao)
        {
            var dbResult = await _db.METAR
                .Where(m => m.ICAO == icao)
                .OrderByDescending(t => t.ValidFrom)
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync();

            var newMetarResponse = new MetarResponse(dbResult);

            return newMetarResponse;



        }
    }
}
