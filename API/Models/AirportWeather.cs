using API.Models.DB;

namespace API.Models
{
    public class AirportWeather
    {
        public METAR Metar { get; set; }
        public TAF Taf { get; set; }
    }
}
