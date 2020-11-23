﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WhenINeedToWork.Services;

namespace WhenINeedToWork.Services.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20201123162441_FK_fix")]
    partial class FK_fix
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WhenINeedToWork.Models.Calendar", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DescriptionPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsGeneralized")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("User_id")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("User_id");

                    b.ToTable("Calendars");
                });

            modelBuilder.Entity("WhenINeedToWork.Models.Event", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Calendar_id")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("End_Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Start_Date")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("Calendar_id");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("WhenINeedToWork.Models.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WhenINeedToWork.Models.Calendar", b =>
                {
                    b.HasOne("WhenINeedToWork.Models.User", "User_")
                        .WithMany("Calendars")
                        .HasForeignKey("User_id");
                });

            modelBuilder.Entity("WhenINeedToWork.Models.Event", b =>
                {
                    b.HasOne("WhenINeedToWork.Models.Calendar", "Calendar_")
                        .WithMany("Events")
                        .HasForeignKey("Calendar_id");
                });
#pragma warning restore 612, 618
        }
    }
}
