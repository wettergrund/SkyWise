namespace API.Models.DTO;

public class PostDBResponse
{
    public bool Succesful { get; set; }
    public string? Message { get; set; }
    public object? Object { get; set; }
}