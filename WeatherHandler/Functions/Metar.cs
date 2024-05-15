using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using WeatherHandler.Services;

namespace WeatherHandler.Functions;

public class Metar
{
    private readonly ILogger _logger;
    private readonly IWxServices _wxServices;

    public Metar(ILoggerFactory loggerFactory, IWxServices wxServices)
    {
        _logger = loggerFactory.CreateLogger<Metar>();
        _wxServices = wxServices;
    }

    [Function("Metar")]
    public async Task Run([TimerTrigger("10 50,20 * * * *")] TimerInfo myTimer)
    {
        var result = await _wxServices.FetchMetar();
        
        if(!result) _logger.LogError("Unable fetch metar"); 
    }
    
    [Function("Taf")]
         public async Task RunTaf([TimerTrigger("30 0,6,12,18 * * * *")] TimerInfo myTimer)
         {
             var result = await _wxServices.FetchTaf();
             
             if(!result) _logger.LogError("Unable fetch TAF"); 
         }
         
    [Function("CleanUp")]
         public async Task RunCleanup([TimerTrigger("0 0 * * * *")] TimerInfo myTimer)
         {
             var result = await _wxServices.FetchTaf();
             
             if(!result) _logger.LogError("Unable fetch TAF"); 
         }
}