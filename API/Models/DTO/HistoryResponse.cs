namespace API.Models.DTO;

public class HistoryResponse
{

    public HistoryResponse()
    {
        SingleAirport = string.IsNullOrEmpty(ArrivalICAO);
    }
    
    private string? _arrIcao;
    
    public int Id { get; set; }
    public string DepartureICAO { get; set; }

    public string? ArrivalICAO
    {
        get => _arrIcao;
        set
        {
            _arrIcao = value;
            SingleAirport = string.IsNullOrEmpty(value);
        }
    }

    public DateTime TimeStamp { get; set; }
    public bool SingleAirport { get; set; } 
}

public class HistoryList
{
    public List<HistoryResponse> List { get; set; } = new List<HistoryResponse>();
}