using API.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace API.Repositories
{
    public class WeatherRepo : IWeatherRepo
    {
        private readonly IMetarRepo _metarRepo;
        private readonly ITafRepo _tafRepo;
        private readonly IDistributedCache _cache;
        public WeatherRepo(IMetarRepo metarRepo, ITafRepo tafRepo, IDistributedCache cache)
        {
            _metarRepo = metarRepo;
            _tafRepo = tafRepo;
            _cache = cache;

        }

        public async Task<AirportWeather> GetAirportWeatherAsync(string icao)
        {

            // Try to fetch data from redis
            var cachedWeather = await GetFromRedisAsync(icao);

            // If no result
            if (cachedWeather is not null) return cachedWeather;
            
            var metar = await _metarRepo.GetMetarAsync(icao);
            var taf = await _tafRepo.GetTafAsync(icao);

            var airportWeather = new AirportWeather { Metar = metar, Taf = taf };
            
            
            // Add to Reids
            await AddToRedis(airportWeather, icao);
            
            return airportWeather;




        }


        private async Task<AirportWeather?> GetFromRedisAsync(string icao)
        {
            string ICAO = icao.ToUpper();

            var cacheResult = _cache.GetString(ICAO);

            if (string.IsNullOrEmpty(cacheResult))
            {
                return null;
            }

            var weatherObject = new AirportWeather();

            try
            {
                weatherObject = JsonConvert.DeserializeObject<AirportWeather>(cacheResult, settings: new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore

                });

                return weatherObject;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to convert cache to object", ex);
            }
            
        }

        private async Task AddToRedis(AirportWeather airportWeather, string icao)
        {
        
            DistributedCacheEntryOptions options = new();
            options.SetAbsoluteExpiration(new TimeSpan(10,0,0,0));
            
            await _cache.SetStringAsync(icao.ToUpper(),JsonConvert.SerializeObject(airportWeather,settings: new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver =  new CamelCasePropertyNamesContractResolver()
            }),options);
            
            Console.WriteLine(icao + "added");
            // todo: log 
            
            
            
            
        }
    }
}
