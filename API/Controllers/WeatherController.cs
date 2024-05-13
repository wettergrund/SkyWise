using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using API.Models;
using API.Models.DB;
using Microsoft.Extensions.Caching.Distributed;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController(IWeatherDataHandler weatherDataHandler, IDistributedCache cache, IConfiguration config, ILoggerFactory log) : ControllerBase
    {

        private ILogger _log = log.CreateLogger<WeatherController>();

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
        [HttpGet("/manual/metartest")] //Function?

        public async Task<IActionResult> Lab()
        {

            await weatherDataHandler.FetchMetar();

            return Ok();
        }


        [HttpGet("/manual/taftest")] //Function?

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
            var redis = config.GetConnectionString("Redis") ?? "noConnectionString";
            
            _log.LogInformation("Redis connection string: " + redis);

            try
            {
                cache.SetString("test", "value");

            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Redis error");
            }



            cache.SetString("test", "Value");
            
            
            return Ok("Success?");
        }
        
    }
}
