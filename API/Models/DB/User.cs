using System.ComponentModel.DataAnnotations;

namespace API.Models.DB
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string ProviderId { get; set; }
        public string? UserEmail { get; set; }

        public virtual ICollection<UserAirportFavorite> Favorites { get; set; }
        public virtual ICollection<UserAirportHistory> History { get; set; }


    }
}
