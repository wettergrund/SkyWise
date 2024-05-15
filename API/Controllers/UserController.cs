using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using API.Models.DTO;
using API.Repositories;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(IUserServices userServices, IHistoryRepo historyRepo) : ControllerBase
    {
        [HttpGet("Recent")]
        public Task<IActionResult> GetRecentSearches()
        {
            throw new NotImplementedException();
        }

        [HttpGet("Favorites")]
        public async Task<IActionResult> GetUserFavorites()
        {
            var userId = await userServices.ValidateUserAsync(User);
            
            var result = await historyRepo.GetUserHistory(userId);

            return Ok(result);
        }

        [HttpPost("NewFavorite")]
        public async Task<IActionResult> SetUserFavorite(string fromIcao, string? toIcao)
        {
            var userId = await userServices.ValidateUserAsync(User); //Todo: Return user object?

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User not found");

            }

            var payload = new FavHistModel()
            {
                UserId = userId,
                From = fromIcao,
                To = toIcao
            };
            var result = await userServices.AddFavorite(payload);
            
            return Ok(result);
        }

        [HttpDelete("RemoveFavorite")]
        public Task<IActionResult> RemoveUserFavorite()
        {
            throw new NotImplementedException();
        }

        [HttpPost("NewHistory")]
        public async Task<IActionResult> AddUserHistory(string fromIcao, string? toIcao)
        {
            var userId = await userServices.ValidateUserAsync(User);
            
             var payload = new FavHistModel()
             {
                 UserId = userId,
                 From = fromIcao,
                 To = toIcao
             };


             var result = await userServices.AddHistory(payload);

             return result.Succesful ? Ok(result) : BadRequest(result);

        }

        [HttpGet("CheckUser")]
        public async Task<IActionResult> CheckUser() {

            var test = await userServices.ValidateUserAsync(User);


            return Ok(test);

        }
    }
}
