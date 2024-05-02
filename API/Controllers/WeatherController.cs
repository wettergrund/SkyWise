using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Distributed;
using NetTopologySuite.Geometries;
using StackExchange.Redis;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController(IWeatherDataHandler weatherDataHandler, IDistributedCache cache) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetCurrentWeather(string ICAO)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            /*TODO:
             * Ceck cache
             * If not in cache, fetch info from API (or DB?)
             * Update cache
             * Return weather data
             */
            var result = await weatherDataHandler.GetWeatherByICAO(ICAO);





            return Ok(result);
        }
        [HttpGet("/manual/metartest")]

        public async Task<IActionResult> Lab()
        {

            await weatherDataHandler.FetchMetar();

            return Ok();
        }


        [HttpGet("/manual/taftest")]

        public async Task<IActionResult> TafLab()
        {

            await weatherDataHandler.FetchTaf();

            return Ok();
        }
        
        [HttpGet("/byline")]
        public async Task<IActionResult> GetByLine(string from, string to)
        {
            var result = await weatherDataHandler.GetWxByLine(from, to);
            return Ok(result);
        }
        
        [HttpGet("/redisTest")]
        public async Task<IActionResult> TestRedis()
        {
            
            cache.SetString("test","value");
            
      
            var test = cache.GetString("test");
            
            
            return Ok(test);
        }
        
    }
}
