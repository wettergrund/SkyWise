﻿using API.Models.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TAF>()
                .OwnsMany(taf => taf.Forcasts, builder => { builder.ToJson(); });

            modelBuilder.Entity<METAR>()
                .OwnsMany(metar => metar.CloudLayers, builder => { builder.ToJson(); });
        }


        public DbSet<Airport> Airport { get; set; }

        public DbSet<TAF> TAF { get; set; }
        public DbSet<METAR> METAR { get; set; }
        //public DbSet<User> User { get; set; }

        //public DbSet<UserAirportFavorite> UserAirportFavorite { get; set; }
        //public DbSet<UserAirportHistory> UserAirportHistory { get; set; }




    }
}
