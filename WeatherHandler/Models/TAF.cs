using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WeatherHandler.Models;

public class TAF
{
    [Key]
    public int Id { get; set; }

    public string RawTAF { get; set; }
    public string ICAO { get; set; }
    public DateTime IssueTime { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public string? Remarks { get; set; }
    public List<Forcast> Forcasts { get; set; } = new(); //Fix typo
        
    [JsonIgnore]
    public virtual Airport Airport { get; set; }
    
}


public class Forcast
{
    public DateTime? ForcastFromTime { get; set; } //Fix typo
    public DateTime? ForcastToTime { get; set; } //Fix typo
    public string? ChangeIndicator { get; set; }
    public DateTime? BecomingTime { get; set; }
    public Probability Probability { get; set; } = Probability.Empty;
    public int WindDirectionDeg { get; set; }
    public int WindSpeedKt { get; set; }
    public int WindGustKt { get; set; }
    public int VisibilityM { get; set; }
    public int VerticalVisibilityFt { get; set; }
    public string WxString { get; set; }
    public List<CloudModel> CloudLayers { get; set; }
}

public enum Probability
{
    Empty,
    P30,
    P40
}