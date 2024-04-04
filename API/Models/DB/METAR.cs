namespace API.Models.DB
{
   public class METAR
   {
      public int Id { get; set; } 
      public string ICAO { get; set; }
      public double Temp { get; set; }
      public double DewPoint { get; set; }
      public int WindDirectionDeg { get; set; }
      public int WindSpeedKt { get; set; }
      public int? WindGustKt { get; set; }
      public bool Auto { get; set; }
      public List<Cloud> CloudLayers { get; set; }
      public string Rules { get; set; }

   } 

   private class Cloud
   {
    public string Cover { get; set; } 
    public int CloudBase { get; set; }
   }
}
