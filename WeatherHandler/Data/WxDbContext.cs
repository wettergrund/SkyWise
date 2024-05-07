using Microsoft.EntityFrameworkCore;
using WeatherHandler.Models;

namespace WeatherHandler.Data;

public class WxDbContext : DbContext
{
    public WxDbContext(DbContextOptions options) : base(options){ }
    
 
    public DbSet<Airport> Airport { get; set; }
    public DbSet<TAF> TAF { get; set; }
    public DbSet<METAR> METAR { get; set; }
    
}