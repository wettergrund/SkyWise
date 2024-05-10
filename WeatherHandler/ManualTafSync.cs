using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using WeatherHandler.Services;

namespace WeatherHandler;

public class ManualTafSync(IWxServices wxServices)
{


    [Function("ManualTafSync")]
    public async Task<bool> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
        FunctionContext executionContext) => await wxServices.FetchTaf();
}