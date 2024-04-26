using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(IUserServices userServices) : ControllerBase
    {
        [HttpGet("Recent")]
        public Task<IActionResult> GetRecentSearches()
        {
            throw new NotImplementedException();
        }

        [HttpGet("Favorites")]
        public Task<IActionResult> GetUserFavorites()
        {
            throw new NotImplementedException();
        }

        [HttpPost("NewFavorite")]
        public async Task<IActionResult> SetUserFavorite(string fromIcao, string? toIcao)
        {
            var userId = await userServices.ValidateUserAsync(User);

            if (userId == null)
            {
                return BadRequest("User not found");

            }

            var result = await userServices.AddFavorite(userId, fromIcao, toIcao);
            
            return Ok(result);
        }

        [HttpDelete("RemoveFavorite")]
        public Task<IActionResult> RemoveUserFavorite()
        {
            throw new NotImplementedException();
        }

        [HttpGet("CheckUser")]
        public async Task<IActionResult> CheckUser() {

            var test = await userServices.ValidateUserAsync(User);


            return Ok(test);

        }
    }
}
