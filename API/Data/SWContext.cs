using API.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class SWContext : DbContext
    {
        public SWContext(DbContextOptions options) : base (options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                add =>
                {
                    add.UseNetTopologySuite();
                }
                );
        }

        public DbSet<User> User { get; set; }

        public DbSet<Airport> Airport { get; set; }

        public DbSet<UserAirportFavorite> UserAirportFavorite { get; set; }
        public DbSet<UserAirportHistory> UserAirportHistory { get; set; }



    }
}
