using WeatherHandler.Models;

namespace WeatherHandler.Repositories;

public interface IRedisHandler
{
   Task<bool> UpdateRedis(METAR metar);
}