namespace API.Models.DB
{
    public class UserAirportFavorite
    {


        public int Id { get; set; }
        public string DepartureICAO { get; set; }
        public string? ArrivalICAO { get; set; }

    }
}
