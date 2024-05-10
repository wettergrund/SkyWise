using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        
        
        services.AddDbContext<WxDbContext>(options =>
                options.UseSqlServer(connectionString),
            ServiceLifetime.Scoped
        );
        
        services.AddScoped<IRepoBase<METAR>, RepoBase<METAR>>();
        services.AddScoped<IRepoBase<TAF>, RepoBase<TAF>>();
        services.AddScoped<IAirportRepo, AirportRepo>();
        services.AddScoped<IMetarRepo, MetarRepo>();
  
        
        services.AddScoped<IWxServices, WxServices>();
        
        
        



        
        

    })
    .Build();

host.Run();