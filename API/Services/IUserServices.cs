using System.Security.Claims;
using API.Models.DTO;

namespace API.Services
{
    public interface IUserServices
    {
        Task<string?> ValidateUserAsync(ClaimsPrincipal claims);
        Task<bool> RemoveUserDataAsync(ClaimsPrincipal claims);

        Task<bool> AddFavorite(FavHistModel model);
        Task<PostDBResponse> AddHistory(FavHistModel model);
        
        

        //Task<bool> GetUserData(string uid);

    }
}
