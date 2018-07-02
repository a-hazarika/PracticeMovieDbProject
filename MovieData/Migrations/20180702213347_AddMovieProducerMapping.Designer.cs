﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieData;

namespace MovieData.Migrations
{
    [DbContext(typeof(MovieDbContext))]
    [Migration("20180702213347_AddMovieProducerMapping")]
    partial class AddMovieProducerMapping
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MovieData.Models.Actor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Bio");

                    b.Property<DateTime>("DOB");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("MiddleName")
                        .HasMaxLength(255);

                    b.Property<int?>("MovieId");

                    b.Property<int>("SexId");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.HasIndex("SexId");

                    b.ToTable("Actors");
                });

            modelBuilder.Entity("MovieData.Models.Gender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Symbol")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Gender");
                });

            modelBuilder.Entity("MovieData.Models.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Plot");

                    b.Property<string>("PosterUrl");

                    b.Property<int?>("ProducerId");

                    b.Property<int?>("ReleaseYear");

                    b.HasKey("Id");

                    b.HasIndex("ProducerId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("MovieData.Models.MovieActorMapping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ActorId");

                    b.Property<int?>("MovieId");

                    b.HasKey("Id");

                    b.HasIndex("ActorId");

                    b.HasIndex("MovieId");

                    b.ToTable("MovieActorMappings");
                });

            modelBuilder.Entity("MovieData.Models.Producer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Bio");

                    b.Property<DateTime>("DOB");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("MiddleName")
                        .HasMaxLength(255);

                    b.Property<int>("SexId");

                    b.HasKey("Id");

                    b.HasIndex("SexId");

                    b.ToTable("Producers");
                });

            modelBuilder.Entity("MovieData.Models.Actor", b =>
                {
                    b.HasOne("MovieData.Models.Movie")
                        .WithMany("Actors")
                        .HasForeignKey("MovieId");

                    b.HasOne("MovieData.Models.Gender", "Sex")
                        .WithMany()
                        .HasForeignKey("SexId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MovieData.Models.Movie", b =>
                {
                    b.HasOne("MovieData.Models.Producer", "Producer")
                        .WithMany()
                        .HasForeignKey("ProducerId");
                });

            modelBuilder.Entity("MovieData.Models.MovieActorMapping", b =>
                {
                    b.HasOne("MovieData.Models.Actor", "Actor")
                        .WithMany()
                        .HasForeignKey("ActorId");

                    b.HasOne("MovieData.Models.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId");
                });

            modelBuilder.Entity("MovieData.Models.Producer", b =>
                {
                    b.HasOne("MovieData.Models.Gender", "Sex")
                        .WithMany()
                        .HasForeignKey("SexId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
