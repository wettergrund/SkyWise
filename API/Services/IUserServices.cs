using API.Models.DB;
using API.Repositories;
using System.Security.Claims;

namespace API.Services
{
    public interface IUserServices
    {
        Task<string?> ValidateUserAsync(ClaimsPrincipal claims);
        Task<bool> RemoveUserDataAsync(ClaimsPrincipal claims);

        Task<bool> AddFavorite(string userId, string fromIcao, string? toIcao);

        //Task<bool> GetUserData(string uid);

    }

    public class UserServices(IUserRepo userRepo, IRepoBase<UserAirportFavorite> favRepo) : IUserServices
    {
        public async Task<string?> ValidateUserAsync(ClaimsPrincipal claims)
        {

            var id = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (id == null) return null;


            var getUser = await userRepo.GetByUidAsync(id);

            if (getUser == null)
            {
                string? email = claims.FindFirst(ClaimTypes.Email)?.Value;

                var newUser = new User()
                {
                    ProviderId = id,
                    UserEmail = email,
                };

                await userRepo.Add(newUser);
                await userRepo.SaveChanges();

            };

            return id;

        }

        public Task<bool> RemoveUserDataAsync(ClaimsPrincipal claims)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddFavorite(string userId, string fromIcao, string? toIcao)
        {
            var user = await userRepo.GetByUidAsync(userId);

            var newFavorite = new UserAirportFavorite { 
                DepartureICAO = fromIcao,  
                User = user
            
            };

            if(!string.IsNullOrEmpty(toIcao)) {
                newFavorite.ArrivalICAO = toIcao;
            }

            await favRepo.Add(newFavorite);
            await favRepo.SaveChanges();

            return true;
        }
    }
}
