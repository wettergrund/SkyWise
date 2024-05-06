using API.Models.DB;
using API.Models.DTO;

namespace API.Models
{
    public class AirportWeather
    {
        public MetarResponse Metar { get; set; }
        public TafResponse? Taf { get; set; }
    }
}
