using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WeatherHandler
{
    public class DbTest
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;
        public DbTest(ILoggerFactory loggerFactory, IConfiguration configuration, IDistributedCache cache)
        {
            _logger = loggerFactory.CreateLogger<DbTest>();
            _configuration = configuration;
            _cache = cache;
        }

        [Function("DbTest")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var cs = _configuration.GetConnectionString("DefaultConnection");

            
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            var test =  await _cache.GetStringAsync("ESSA");
            response.WriteString("Welcome to Azure Functions!");
            response.WriteString(cs);


            return response;
        }
    }
}
