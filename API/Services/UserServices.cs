using API.Models.DB;
using API.Repositories;
using System.Security.Claims;
using API.Models.DTO;

namespace API.Services
{
    public class UserServices(IUserRepo userRepo, IRepoBase<UserAirportFavorite> favRepo, IRepoBase<UserAirportHistory> histRepo) : IUserServices
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

        public async Task<PostDBResponse> AddHistory(FavHistModel model)
        {
            var user = await GetUserAsync(model.UserId);
           
            
            
            var newHistory = new UserAirportHistory()
            {
                DepartureICAO = model.From,
                User = user
                
            };

            if (!string.IsNullOrEmpty(model.To)) newHistory.ArrivalICAO = model.To;

            await histRepo.Add(newHistory);
            await histRepo.SaveChanges();

            return new PostDBResponse() { Object = newHistory };
            
        
            throw new NotImplementedException();
        }

        public async Task<bool> AddFavorite(FavHistModel model)
        {
            var user = await userRepo.GetByUidAsync(model.UserId);

            var newFavorite = new UserAirportFavorite { 
                DepartureICAO = model.From,  
                User = user
            
            };

            if(!string.IsNullOrEmpty(model.To)) {
                newFavorite.ArrivalICAO = model.To;
            }

            await favRepo.Add(newFavorite);
            await favRepo.SaveChanges();

            return true;
        }

        private async Task<User?> GetUserAsync(string uid)
        {
            return await userRepo.GetByUidAsync(uid);
        }
    }
}
