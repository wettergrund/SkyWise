using Owin;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Services;

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
