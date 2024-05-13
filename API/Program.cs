using API.Data;
using API.Models.DB;
using API.Repositories;
using API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.IdentityModel.Protocols.Configuration;
using StackExchange.Redis;

namespace API
{

    public class Program
    {
        public static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;


            var builder = WebApplication.CreateBuilder(args);

            SetupAuhentication(builder);

            builder.Services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNameCaseInsensitive = true);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            // DB context 
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            string redisConnectionSting = builder.Configuration.GetConnectionString("RedisDefaultConnection") ??
                                          builder.Configuration["RedisDefaultConnection"] ?? "Invalid redis connection string";

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionSting;
                
            });
            
            builder.Services.AddDbContext<SWContext>
            (
                options =>
                   options.UseSqlServer(connectionString),
                   ServiceLifetime.Scoped
            );


            builder.Services.AddScoped<IWeatherDataHandler, WeatherDataHandler>();
            builder.Services.AddScoped<IUserServices, UserServices>();

            builder.Services.AddScoped<IWeatherRepo, WeatherRepo>();
            builder.Services.AddScoped<IMetarRepo, MetarRepo>();
            builder.Services.AddScoped<ITafRepo, TafRepo>();
            builder.Services.AddScoped<IAirportRepo, AirportRepo>();

            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<IRepoBase<UserAirportFavorite>, RepoBase<UserAirportFavorite>>();
            builder.Services.AddScoped<IRepoBase<UserAirportHistory>, RepoBase<UserAirportHistory>>();





            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // if (app.Environment.IsDevelopment())
            // {
                app.UseSwagger();
                app.UseSwaggerUI();
            // }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void SetupAuhentication(WebApplicationBuilder builder)
        {
            string productId = builder.Configuration["Auth:ProductId"] ?? throw new InvalidOperationException("Add Firebase prtoductID to configuration");
            string googleEndpoint = "https://securetoken.google.com/" + productId;

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                .AddJwtBearer(options =>
                {
                    options.Authority = googleEndpoint;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = googleEndpoint,
                        ValidateAudience = true,
                        ValidAudience = productId,
                        ValidateLifetime = true
                    };
                });
            }




    }
}
