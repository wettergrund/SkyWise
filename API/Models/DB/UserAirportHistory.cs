namespace API.Models.DB
{
    public class UserAirportHistory
    {
        public int Id { get; set; }
        public string DepartureICAO { get; set; }
        public string? ArrivalICAO { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public virtual User User { get; set; }
    }
}
