using API.Models.DB;

namespace API.Models.DTO;



public class TafResponse
{
    public TafResponse()
    {
        
    }
    public TafResponse(TAF dbModel)
    {
        
        RawTAF = dbModel.RawTAF;
        ICAO = dbModel.ICAO;
        IssueTime = dbModel.IssueTime;
        ValidFrom = dbModel.ValidFrom;
        ValidTo = dbModel.ValidTo;
        Remarks = dbModel.Remarks;
        Forcasts = dbModel.Forcasts;

    }
    public string RawTAF { get; set; }
    public string ICAO { get; set; }
    public DateTime IssueTime { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public string? Remarks { get; set; }
    public List<Forcast> Forcasts { get; set; } = new(); //Fix typo


}