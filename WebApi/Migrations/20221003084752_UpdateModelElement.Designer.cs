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
    [Migration("20221003084752_UpdateModelElement")]
    partial class UpdateModelElement
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("WebApi.Models.CciEePp", b =>
                {
                    b.Property<int>("CciEePpId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("DefinitionEe")
                        .HasColumnType("longtext");

                    b.Property<string>("DefinitionEn")
                        .HasColumnType("longtext");

                    b.Property<string>("Level1")
                        .HasColumnType("longtext");

                    b.Property<string>("Level2")
                        .HasColumnType("longtext");

                    b.Property<string>("Level3")
                        .HasColumnType("longtext");

                    b.Property<string>("Level4")
                        .HasColumnType("longtext");

                    b.Property<string>("TermEe")
                        .HasColumnType("longtext");

                    b.Property<string>("TermEn")
                        .HasColumnType("longtext");

                    b.HasKey("CciEePpId");

                    b.ToTable("CciEePps");
                });

            modelBuilder.Entity("WebApi.Models.ModelElement", b =>
                {
                    b.Property<int>("ModelElementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ExpressId")
                        .HasColumnType("int");

                    b.Property<string>("Guid")
                        .HasColumnType("longtext");

                    b.Property<string>("IfcStorey")
                        .HasColumnType("longtext");

                    b.Property<string>("IfcType")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("ObjectType")
                        .HasColumnType("longtext");

                    b.HasKey("ModelElementId");

                    b.ToTable("ModelElements");
                });

            modelBuilder.Entity("WebApi.Models.ModelElementInWorkPackage", b =>
                {
                    b.Property<int>("ModelElementInWorkPackageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ModelElementId")
                        .HasColumnType("int");

                    b.Property<int>("WorkPackageId")
                        .HasColumnType("int");

                    b.HasKey("ModelElementInWorkPackageId");

                    b.HasIndex("ModelElementId");

                    b.HasIndex("WorkPackageId");

                    b.ToTable("ModelElementInWorkPackages");
                });

            modelBuilder.Entity("WebApi.Models.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("IfcFileName")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("ProjectId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("WebApi.Models.WorkPackage", b =>
                {
                    b.Property<int>("WorkPackageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CciEePpId")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("WorkPackageId");

                    b.HasIndex("CciEePpId");

                    b.HasIndex("ProjectId");

                    b.ToTable("WorkPackages");
                });

            modelBuilder.Entity("WebApi.Models.ModelElementInWorkPackage", b =>
                {
                    b.HasOne("WebApi.Models.ModelElement", "ModelElement")
                        .WithMany("ModelElementInWorkPackages")
                        .HasForeignKey("ModelElementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi.Models.WorkPackage", "WorkPackage")
                        .WithMany("ModelElementInWorkPackages")
                        .HasForeignKey("WorkPackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ModelElement");

                    b.Navigation("WorkPackage");
                });

            modelBuilder.Entity("WebApi.Models.WorkPackage", b =>
                {
                    b.HasOne("WebApi.Models.CciEePp", "CciEePp")
                        .WithMany("WorkPackages")
                        .HasForeignKey("CciEePpId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi.Models.Project", "Project")
                        .WithMany("WorkPackages")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CciEePp");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("WebApi.Models.CciEePp", b =>
                {
                    b.Navigation("WorkPackages");
                });

            modelBuilder.Entity("WebApi.Models.ModelElement", b =>
                {
                    b.Navigation("ModelElementInWorkPackages");
                });

            modelBuilder.Entity("WebApi.Models.Project", b =>
                {
                    b.Navigation("WorkPackages");
                });

            modelBuilder.Entity("WebApi.Models.WorkPackage", b =>
                {
                    b.Navigation("ModelElementInWorkPackages");
                });
#pragma warning restore 612, 618
        }
    }
}
