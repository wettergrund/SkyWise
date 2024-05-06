namespace API.Models.DTO;
using NetTopologySuite.Geometries;


public class AirportListResponse
{
    public string ICAO { get; set; }
    public Point Location { get; set; }
}