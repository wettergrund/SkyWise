using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using WeatherHandler.Repositories;

namespace WeatherHandler;

public class WeatherCleanupManual(IMetarRepo metarRepo)
{
   
    [Function("WeatherCleanupManual")]
    public bool Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
        FunctionContext executionContext)
    {

        return metarRepo.RemoveOldMetars();

    }
}