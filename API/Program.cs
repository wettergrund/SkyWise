using API.Data;
using API.Repositories;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Auth
            var auth0 = builder.Configuration.GetSection("auth0");
            string auth0Domain = auth0["Domain"];
            string auth0ClientId = auth0["ClientId"];
            string auth0RedirectUri = auth0["RedirectUri"];
            string auth0PostLogoutRedirectUri = auth0["PostLogoutRedirectUri"];

            // DB context 
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<SWContext>
            (
            options =>
               options.UseSqlServer(connectionString),
               ServiceLifetime.Scoped
            );


            builder.Services.AddScoped<IWeatherDataHandler, WeatherDataHandler>();
            builder.Services.AddScoped<IWeatherRepo, WeatherRepo>();
            builder.Services.AddScoped<IMetarRepo, MetarRepo>();
            builder.Services.AddScoped<ITafRepo, TafRepo>();

            builder.Services.AddScoped<IAirportRepo, AirportRepo>();







            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }




    }
}
