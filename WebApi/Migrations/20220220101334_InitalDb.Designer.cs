﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApi;

#nullable disable

namespace WebApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220220101334_InitalDb")]
    partial class InitalDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("WebApi.Models.IfcElement", b =>
                {
                    b.Property<int>("IfcElementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("IfcName")
                        .HasColumnType("longtext");

                    b.HasKey("IfcElementId");

                    b.ToTable("IfcElements");
                });
#pragma warning restore 612, 618
        }
    }
}
