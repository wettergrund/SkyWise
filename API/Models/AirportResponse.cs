using API.Models.DB;
using NetTopologySuite.Geometries;

namespace API.Models;

public class AirportResponse
{
    public AirportResponse(Airport dbProperties)
    {
        ICAO = dbProperties.ICAO;
        Latitude = dbProperties.Location.Y;
        Longitude = dbProperties.Location.X;
    }
    public string ICAO { get; private set; }
    public double Latitude { get; set; } //Y
    public double Longitude { get; set; } //X

}