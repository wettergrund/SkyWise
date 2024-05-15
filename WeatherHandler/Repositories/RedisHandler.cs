using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WeatherHandler.Models;
using WeatherHandler.Models.DTO;

namespace WeatherHandler.Repositories;

public class RedisHandler(IDistributedCache redis) : IRedisHandler
{
    public async Task<bool> UpdateRedis(METAR metar)
    {
        /*
         * 1. Check if cache exist. If not return false;
         * 2. change the METAR
         * 3. Post to redis again
         */

        string icao = metar.ICAO.ToUpper();
        

        var weatherObject = await GetWeather(icao);
        if (weatherObject is null) return false;

        var updatedMetar = new MetarResponse(metar);

        weatherObject.Metar = updatedMetar;

        
            await redis.SetStringAsync(icao.ToUpper(),JsonConvert.SerializeObject(weatherObject,settings: new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver =  new CamelCasePropertyNamesContractResolver()
            }));

        return true;
    }

    public async Task<bool> UpdateRedis(TAF taf)
    {
        string icao = taf.ICAO.ToUpper();

        var weatherObject = await GetWeather(icao);

        if (weatherObject is null) return false;
        var updatedTaf = new TafResponse(taf);

        weatherObject.Taf = updatedTaf;
        
            await redis.SetStringAsync(icao.ToUpper(),JsonConvert.SerializeObject(weatherObject,settings: new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver =  new CamelCasePropertyNamesContractResolver()
            }));
            
            
            return true;

    }

    private async Task<AirportWeather?> GetWeather(string icao)
    {
        
        var weather = await redis.GetStringAsync(icao);

        if (string.IsNullOrEmpty(weather)) return null;
        
        
        var weatherObject = new AirportWeather();

        try
        {

            weatherObject = JsonConvert.DeserializeObject<AirportWeather>(weather,
                settings: new JsonSerializerSettings()
                {

                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,


                });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }

        return weatherObject;

    }
}