using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : Controller
    {
 
        [HttpGet]
        public async Task<IActionResult> GetCurrentWeather(string ICAO) {

            /*TODO:
             * Ceck cache
             * If not in cache, fetch info from API (or DB?)
             * Update cache
             * Return weather data
             */




            return Ok(ICAO);
        }
    }
}
