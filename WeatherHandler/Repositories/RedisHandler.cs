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
        
        var getMetar = await redis.GetStringAsync(metar.ICAO);

        if (string.IsNullOrEmpty(getMetar)) return false;

        var weatherObject = new AirportWeather();

        weatherObject = JsonConvert.DeserializeObject<AirportWeather>(getMetar, settings: new JsonSerializerSettings()
                                                                                              {
                                                      
                                                                                                  ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                                                                                  
                                                                              
                                                                                              });

        var updatedMetar = new MetarResponse(metar);

        weatherObject.Metar = updatedMetar;

        DistributedCacheEntryOptions options = new();
                     options.SetAbsoluteExpiration(new TimeSpan(10,0,0,0));
                     
        await redis.GetStringAsync(icao);
        
            await redis.SetStringAsync(icao.ToUpper(),JsonConvert.SerializeObject(weatherObject,settings: new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver =  new CamelCasePropertyNamesContractResolver()
            }));

        return true;
    }
}