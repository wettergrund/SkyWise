using NetTopologySuite.Geometries;

namespace WeatherHandler.Models;

public class Airport
{
    public int Id { get; set; }
    public string ICAO { get; set; }
    public Point Location { get; set; }


    public virtual ICollection<METAR> Metars { get; set; }
    public virtual ICollection<TAF> Tafs { get; set; }
}