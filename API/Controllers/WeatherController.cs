using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController(IWeatherDataHandler weatherDataHandler) : Controller
    {


        [HttpGet]
        public async Task<IActionResult> GetCurrentWeather(string ICAO)
        {

            /*TODO:
             * Ceck cache
             * If not in cache, fetch info from API (or DB?)
             * Update cache
             * Return weather data
             */
            var result = await weatherDataHandler.GetWeatherByICAO(ICAO);





            return Ok(result);
        }

        [HttpGet("/manual/addtaf")]

        public async Task<IActionResult> AddTaf()
        {

            var result = await weatherDataHandler.AddTaf();

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
    }
}
