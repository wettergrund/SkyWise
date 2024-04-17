using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace API.Models.DB
{
    public class Airport
    {
        [Key]
        public int Id { get; set; }
        public string ICAO { get; set; }
        public Point Location { get; set; }


        public virtual ICollection<METAR> Metars { get; set; }
        public virtual ICollection<TAF> Tafs { get; set; }

    }
}
