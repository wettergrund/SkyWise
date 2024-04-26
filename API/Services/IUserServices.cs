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
}
