using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;
using DataLayer.Models.Test;

namespace DataLayer
{
    public class ImdbContext : DbContext
    {

        const string ConnectionString = "host=cit.ruc.dk;db=cit09;uid=cit09;pwd=8wUBnJ0Lw4Zn"; // needs changing

        public DbSet<Title> Titles { get; set; }
        public DbSet<Persons> Persons { get; set; }
        public DbSet<TitleGenre> TitleGenres { get; set; }
        public DbSet<Similar_Title> SimilarMovies { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            optionsBuilder.UseNpgsql(ConnectionString);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Title>().ToTable("titles");
            modelBuilder.Entity<Title>().HasKey(x => x.TitleId);
            modelBuilder.Entity<Title>().Property(x => x.TitleId).HasColumnName("title_ID");
            modelBuilder.Entity<Title>().Property(x => x.Type).HasColumnName("type");
            modelBuilder.Entity<Title>().Property(x => x.PrimaryTitle).HasColumnName("primarytitle");
            modelBuilder.Entity<Title>().Property(x => x.OriginalTitle).HasColumnName("originaltitle");
            modelBuilder.Entity<Title>().Property(x => x.IsAdult).HasColumnName("isadult");
            modelBuilder.Entity<Title>().Property(x => x.StartYear).HasColumnName("startyear");
            modelBuilder.Entity<Title>().Property(x => x.EndYear).HasColumnName("endyear");
            modelBuilder.Entity<Title>().Property(x => x.RunTimeMinutes).HasColumnName("runtimeminutes");
            modelBuilder.Entity<Title>().Property(x => x.Poster).HasColumnName("poster");
            modelBuilder.Entity<Title>().Property(x => x.Plot).HasColumnName("plot");
            modelBuilder.Entity<Title>().Property(x => x.AverageRating).HasColumnName("averagerating");
            modelBuilder.Entity<Title>().Property(x => x.NumVotes).HasColumnName("numvotes");

            modelBuilder.Entity<TitleGenre>().ToTable("title_genres");
            modelBuilder.Entity<TitleGenre>().HasKey(x => new { x.TitleId, x.Genre });
            modelBuilder.Entity<TitleGenre>().Property(x => x.TitleId).HasColumnName("title_ID");
            modelBuilder.Entity<TitleGenre>().Property(x => x.Genre).HasColumnName("genre");

            modelBuilder.Entity<Persons>().ToTable("persons");
            modelBuilder.Entity<Persons>().HasKey(x => x.PersonId);
            modelBuilder.Entity<Persons>().Property(x => x.PersonId).HasColumnName("person_ID");
            modelBuilder.Entity<Persons>().Property(x => x.Name).HasColumnName("name");
            modelBuilder.Entity<Persons>().Property(x => x.BirthYear).HasColumnName("birthyear");
            modelBuilder.Entity<Persons>().Property(x => x.DeathYear).HasColumnName("deathyear");

            modelBuilder.Entity<Similar_Title>().HasNoKey();
            modelBuilder.Entity<Similar_Title>().Property(x => x.TitleId).HasColumnName("similar_movies");


        }
    }
}
