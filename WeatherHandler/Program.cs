using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherHandler.Data;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        var env = context.HostingEnvironment;
        var config = context.Configuration;
        var connectionString = config["DefaultConnectio"];
        
        services.AddDbContext<WxDbContext>(options =>
                options.UseSqlServer(connectionString),
            ServiceLifetime.Scoped
        );

    })
    .Build();

host.Run();