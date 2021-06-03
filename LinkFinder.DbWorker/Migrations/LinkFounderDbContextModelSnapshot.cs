﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using LinkFinder.DatabaseWorker;

namespace LinkFounder.DbSaver.Migrations
{
    [DbContext(typeof(LinkFinderDbContext))]
    partial class LinkFounderDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LinkFounder.DbWorker.Models.Result", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
                b.Property<int>("TestId")
                    .HasColumnType("int");
                b.Property<int>("TimeResponse")
                    .HasColumnType("int");
                b.Property<string>("Url")
                    .HasColumnType("nvarchar(max)");
                b.HasKey("Id");
                b.HasIndex("TestId");
                b.ToTable("Results");
            });
            modelBuilder.Entity("LinkFinder.DbWorker.Models.Test", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
                b.Property<DateTime>("TimeCreated")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnType("datetime2")
                    .HasComputedColumnSql("CONVERT(date,GETUTCDATE())");
                b.Property<string>("Url")
                    .HasColumnType("nvarchar(max)");
                b.HasKey("Id");
                b.ToTable("Tests");
            });
            modelBuilder.Entity("LinkFinder.DbWorker.Models.Result", b =>
            {
                b.HasOne("LinkFinder.DbSaver.Models.Test", "Test")
                    .WithMany("Results")
                    .HasForeignKey("TestId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
                b.Navigation("Test");
            });
               modelBuilder.Entity("LinkFinder.DbWorker.Models.Test", b =>
                {
                    b.Navigation("Results");
                });
#pragma warning restore 612, 618
        }
    }
}