using API.Data;
using API.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class UserRepo : RepoBase<User>, IUserRepo
    {
        public UserRepo(SWContext context) : base(context)
        {
        }

        public async Task<User> GetByUidAsync(string uid)
        {
            User? userFromDb = await _db.User
                .Where(u => u.ProviderId == uid)
                .FirstOrDefaultAsync();

            return userFromDb;

        }
    }
}
