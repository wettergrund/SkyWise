using API.Models.DB;

namespace API.Models.DTO;

public class MetarResponse
{
    public MetarResponse()
    {
        
    }
    public MetarResponse(METAR dbModel)
    {
        RawMetar = dbModel.RawMetar;
        ICAO = dbModel.ICAO;
        ValidFrom = dbModel.ValidFrom;
        Temp = dbModel.Temp;
        DewPoint = dbModel.DewPoint;
        WindDirectionDeg = dbModel.WindDirectionDeg;
        WindSpeedKt = dbModel.WindSpeedKt;
        WindGustKt = dbModel.WindGustKt;
        VisibilityM = dbModel.VisibilityM;
        QNH = dbModel.QNH;
        VerticalVisibilityFt = dbModel.VerticalVisibilityFt;
        WxString = dbModel.WxString;
        Auto = dbModel.Auto;
        CloudLayers = dbModel.CloudLayers;
        Rules = dbModel.Rules;

    }
    
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
}