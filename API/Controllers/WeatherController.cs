using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController(IWeatherDataHandler weatherDataHandler) : ControllerBase
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

        //[HttpGet("/manual/addtaf")]

        //public async Task<IActionResult> AddTaf()
        //{

        //    var result = await weatherDataHandler.AddTaf();

        //    return Ok(result);
        //}

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
    }
}
