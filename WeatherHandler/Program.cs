using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.Configuration;
using Newtonsoft.Json.Linq;
using WeatherHandler.Data;
using WeatherHandler.Models;
using WeatherHandler.Repositories;
using WeatherHandler.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        var env = context.HostingEnvironment;
        var config = context.Configuration;
        string? connectionString = config["DefaultConnection"];

        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine("Connection string incorrect");
        }
        else
        {
            Console.WriteLine("Connection string: " + connectionString);
        }

        string redisConnection = config["RedisConnection"] ?? throw new InvalidConfigurationException("Redis connection string missing");

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnection;
        });
        
        services.AddDbContext<WxDbContext>(options =>
                options.UseSqlServer(connectionString),
            ServiceLifetime.Scoped
        );
        
        services.AddScoped<IRepoBase<METAR>, RepoBase<METAR>>();
        services.AddScoped<IRepoBase<TAF>, RepoBase<TAF>>();
        services.AddScoped<IAirportRepo, AirportRepo>();
        services.AddScoped<IMetarRepo, MetarRepo>();
        services.AddScoped<IRedisHandler, RedisHandler>();
  
        
        services.AddScoped<IWxServices, WxServices>();





        services.Configure<JsonSerializerOptions>(settings =>
        {
            settings.PropertyNameCaseInsensitive = true;
        });




    })
    .Build();

host.Run();