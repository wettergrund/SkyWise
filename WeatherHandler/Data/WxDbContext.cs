using Microsoft.EntityFrameworkCore;
using WeatherHandler.Models;
using Newtonsoft.Json;

namespace WeatherHandler.Data;

public class WxDbContext : DbContext
{
    public WxDbContext(DbContextOptions options) : base(options){ }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            add =>
            {
                add.UseNetTopologySuite();
            }
        );
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TAF>()
            .OwnsMany(taf => taf.Forcasts, builder =>
            {
                builder.ToJson();
                builder.Property(f => f.CloudLayers)
                    .HasConversion(
                        v => JsonConvert.SerializeObject(v),
                        v => JsonConvert.DeserializeObject<List<CloudModel>>(v));

            });



        modelBuilder.Entity<METAR>()
            .OwnsMany(metar => metar.CloudLayers, builder => { builder.ToJson(); });

        modelBuilder.Entity<Airport>().HasIndex(col => col.ICAO).IsUnique();

    }
 
    public DbSet<Airport> Airport { get; set; }
    public DbSet<TAF> TAF { get; set; }
    public DbSet<METAR> METAR { get; set; }
    
}