namespace WeatherHandler.Models.DTO;

public class AirportWeather
{
    
        public MetarResponse Metar { get; set; }
        public TafResponse? Taf { get; set; }
}