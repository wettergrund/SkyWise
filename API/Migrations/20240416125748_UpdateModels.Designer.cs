﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;

#nullable disable

namespace API.Migrations
{
    [DbContext(typeof(SWContext))]
    [Migration("20240416125748_UpdateModels")]
    partial class UpdateModels
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("API.Models.DB.Airport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ICAO")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Point>("Location")
                        .IsRequired()
                        .HasColumnType("geography");

                    b.HasKey("Id");

                    b.HasIndex("ICAO")
                        .IsUnique();

                    b.ToTable("Airport");
                });

            modelBuilder.Entity("API.Models.DB.METAR", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AirportId")
                        .HasColumnType("int");

                    b.Property<bool>("Auto")
                        .HasColumnType("bit");

                    b.Property<double>("DewPoint")
                        .HasColumnType("float");

                    b.Property<string>("ICAO")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("QNH")
                        .HasColumnType("float");

                    b.Property<string>("RawMetar")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rules")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Temp")
                        .HasColumnType("float");

                    b.Property<DateTime>("ValidFrom")
                        .HasColumnType("datetime2");

                    b.Property<int>("VerticalVisibilityFt")
                        .HasColumnType("int");

                    b.Property<int>("VisibilityM")
                        .HasColumnType("int");

                    b.Property<int>("WindDirectionDeg")
                        .HasColumnType("int");

                    b.Property<int?>("WindGustKt")
                        .HasColumnType("int");

                    b.Property<int>("WindSpeedKt")
                        .HasColumnType("int");

                    b.Property<string>("WxString")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AirportId");

                    b.ToTable("METAR");
                });

            modelBuilder.Entity("API.Models.DB.TAF", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AirportId")
                        .HasColumnType("int");

                    b.Property<string>("ICAO")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("IssueTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("RawTAF")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ValidFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ValidTo")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AirportId");

                    b.ToTable("TAF");
                });

            modelBuilder.Entity("API.Models.DB.METAR", b =>
                {
                    b.HasOne("API.Models.DB.Airport", "Airport")
                        .WithMany("Metars")
                        .HasForeignKey("AirportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("API.Models.CloudModel", "CloudLayers", b1 =>
                        {
                            b1.Property<int>("METARId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            b1.Property<int>("CloudBase")
                                .HasColumnType("int");

                            b1.Property<string>("CloudType")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Cover")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("METARId", "Id");

                            b1.ToTable("METAR");

                            b1.ToJson("CloudLayers");

                            b1.WithOwner()
                                .HasForeignKey("METARId");
                        });

                    b.Navigation("Airport");

                    b.Navigation("CloudLayers");
                });

            modelBuilder.Entity("API.Models.DB.TAF", b =>
                {
                    b.HasOne("API.Models.DB.Airport", "Airport")
                        .WithMany("Tafs")
                        .HasForeignKey("AirportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("API.Models.DB.Forcast", "Forcasts", b1 =>
                        {
                            b1.Property<int>("TAFId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            b1.Property<DateTime?>("BecomingTime")
                                .HasColumnType("datetime2");

                            b1.Property<string>("ChangeIndicator")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("CloudLayers")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<DateTime>("ForcastFromTime")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime>("ForcastToTime")
                                .HasColumnType("datetime2");

                            b1.Property<int>("Probability")
                                .HasColumnType("int");

                            b1.Property<int>("VerticalVisibilityFt")
                                .HasColumnType("int");

                            b1.Property<int>("VisibilityM")
                                .HasColumnType("int");

                            b1.Property<int>("WindDirectionDeg")
                                .HasColumnType("int");

                            b1.Property<int>("WindGustKt")
                                .HasColumnType("int");

                            b1.Property<int>("WindSpeedKt")
                                .HasColumnType("int");

                            b1.Property<string>("WxString")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("TAFId", "Id");

                            b1.ToTable("TAF");

                            b1.ToJson("Forcasts");

                            b1.WithOwner()
                                .HasForeignKey("TAFId");
                        });

                    b.Navigation("Airport");

                    b.Navigation("Forcasts");
                });

            modelBuilder.Entity("API.Models.DB.Airport", b =>
                {
                    b.Navigation("Metars");

                    b.Navigation("Tafs");
                });
#pragma warning restore 612, 618
        }
    }
}
