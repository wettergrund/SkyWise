using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.DB
{
    public class TAF
    {
        [Key]
        public int Id { get; set; }
        public string ICAO { get; set; }
        public DateTime IssueTime { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string? Remarks { get; set; }

        public List<Forcast> Forcasts { get; set; } = new();



    }
    
    public class Forcast
    {
        public DateTime ForcastFromTime { get; set; }
        public DateTime ForcastToTime { get; set; }
        public string ChangeIndicator { get; set;}

        public DateTime BecomingTime { get; set;}
        public Probability Probability { get; set; } = Probability.Empty;
        public int WindDirectionDeg { get; set; }
        public int WindSpeedKt { get; set; }
        public int WindGustKt { get; set; }
        public int VisibilityM { get; set; }
        public int QNH { get; set; }
        public int VerticalVisibilityFt { get; set; }
        public string WxString { get; set; }
        public List<TAFCloud> CloudLayers { get; set; }


    }
    [NotMapped]
    public class TAFCloud
    {
        public string Cover { get; set; }
        public int CloudBase { get; set; }
        public string CloudType { get; set; }

    }

    public enum Probability
    {
        Empty,
        P30,
        P40

    }
}
