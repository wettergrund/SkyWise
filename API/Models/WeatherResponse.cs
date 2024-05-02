namespace API.Models;

public class WeatherResponse
{
    public AirportResponse AirportInfo { get; set; }
    public AirportWeather Weather { get; set; }
}