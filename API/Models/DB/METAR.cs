using System.ComponentModel.DataAnnotations;

namespace API.Models.DB
{
    public class METAR
    {
        [Key]
        public int Id { get; set; }
        public string RawMetar { get; set; }
        public string ICAO { get; set; }

        public DateTime ValidFrom { get; set; }
        public double Temp { get; set; }
        public double DewPoint { get; set; }
        public int WindDirectionDeg { get; set; }
        public int WindSpeedKt { get; set; }
        public int? WindGustKt { get; set; }
        public int VisibilityM { get; set; }
        public double QNH { get; set; }
        public int VerticalVisibilityFt { get; set; }
        public string WxString { get; set; }
        public bool Auto { get; set; }

        public List<CloudModel> CloudLayers { get; set; }
        public string Rules { get; set; }


        public virtual Airport Airport { get; set; }


    }


    public class METARCloud
    {
        public string Cover { get; set; }
        public int CloudBase { get; set; }
        public string CloudType { get; set; }


    }
}
