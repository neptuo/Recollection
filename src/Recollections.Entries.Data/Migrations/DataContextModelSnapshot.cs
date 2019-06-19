﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Neptuo.Recollections.Entries;

namespace Neptuo.Recollections.Entries.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("Neptuo.Recollections.Entries.Entry", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<string>("Text");

                    b.Property<string>("Title");

                    b.Property<string>("UserId");

                    b.Property<DateTime>("When");

                    b.HasKey("Id");

                    b.ToTable("Entries");
                });

            modelBuilder.Entity("Neptuo.Recollections.Entries.Image", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("EntryId");

                    b.Property<string>("FileName");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("EntryId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Neptuo.Recollections.Entries.Image", b =>
                {
                    b.HasOne("Neptuo.Recollections.Entries.Entry", "Entry")
                        .WithMany()
                        .HasForeignKey("EntryId");
                });
#pragma warning restore 612, 618
        }
    }
}