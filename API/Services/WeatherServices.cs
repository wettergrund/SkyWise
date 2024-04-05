using API.Data;
using API.Models.DB;

namespace API.Services
{
  public interface IWeatherDataHandler
  {

    public Task<string> GetWeatherByICAO(string ICAO);
      
  }   

  class WeatherDataHandler : IWeatherDataHandler
  {
        private SWContext _context;
        public WeatherDataHandler(SWContext context)
        {
            _context = context;
            
        }
        public async Task<string> GetWeatherByICAO(string ICAO){

            //Only for test
            var metar = new METAR()
            {
                ICAO = "ESOW",
                RawMetar = "ESOW 050820Z 06007KT 040V100 4500 -SN BKN006 01/M00 Q1005=",
                Temp = 1,
                DewPoint = 0,
                WindDirectionDeg = 60,
                WindSpeedKt = 7,
                VisibilityM = 4500,
                QNH = 1005,
                VerticalVisibilityFt = 0,
                WxString = "-SN",
                Auto = false,
                CloudLayers =
                {
                    new(){Cover = "BKN", CloudBase = 600, CloudType = ""}
                },
                Rules = "IFR"
                

            };

            _context.METAR.Add(metar);
            _context.SaveChanges();

      return metar.RawMetar;

    }    
  }
}
