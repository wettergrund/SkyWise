using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
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
        public Task<IActionResult> SetUserFavorite()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("RemoveFavorite")]
        public Task<IActionResult> RemoveUserFavorite()
        {
            throw new NotImplementedException();
        }
    }
}
