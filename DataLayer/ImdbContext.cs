using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;
using DataLayer.Models.Test;
using Microsoft.Extensions.Logging;

namespace DataLayer
{
    public class ImdbContext : DbContext
    {

        const string ConnectionString = "host=cit.ruc.dk;db=cit09;uid=cit09;pwd=8wUBnJ0Lw4Zn"; // needs changing

        public DbSet<Title> Titles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<BookmarkPerson> BookmarkPersons {get; set;}
        public DbSet<BookmarkTitle> BookmarkTitles { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<TitleGenre> TitleGenres { get; set; }
        public DbSet<Similar_Title> SimilarMovies { get; set; }
        



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            optionsBuilder.UseNpgsql(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // titles
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

            // similar titles
            modelBuilder.Entity<Similar_Title>().HasNoKey();
            modelBuilder.Entity<Similar_Title>().Property(x => x.TitleId).HasColumnName("title_ID");
            modelBuilder.Entity<Similar_Title>().Property(x => x.PrimaryTitle).HasColumnName("primarytitle");

            //users
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<User>().HasKey(x => x.Username);
            modelBuilder.Entity<User>().Property(x => x.Username).HasColumnName("username");
            modelBuilder.Entity<User>().Property(x => x.Password).HasColumnName("password");

            modelBuilder.Entity<BookmarkPerson>().ToTable("bookmarkperson");
            //modelBuilder.Entity<BookmarkPerson>().HasKey(x => x.UserName);
            modelBuilder.Entity<BookmarkPerson>().HasNoKey();
            modelBuilder.Entity<BookmarkPerson>().Property(x => x.UserName).HasColumnName("username");
            modelBuilder.Entity<BookmarkPerson>().Property(x => x.PersonName).HasColumnName("name");
            modelBuilder.Entity<BookmarkPerson>().Property(x => x.Timestamp).HasColumnName("timestamp");

            modelBuilder.Entity<BookmarkTitle>().ToTable("bookmarktitle");
            modelBuilder.Entity<BookmarkTitle>().HasKey(x => x.Username);
            modelBuilder.Entity<BookmarkTitle>().Property(x => x.Username).HasColumnName("username");
            modelBuilder.Entity<BookmarkTitle>().Property(x => x.Primarytitle).HasColumnName("primarytitle");
            modelBuilder.Entity<BookmarkTitle>().Property(x => x.Timestamp).HasColumnName("timestamp");

            modelBuilder.Entity<Rating>().ToTable("ratings");
            modelBuilder.Entity<Rating>().HasKey(x => new { x.Username, x.Primarytitle });
            modelBuilder.Entity<Rating>().Property(x => x.Username).HasColumnName("username");
            modelBuilder.Entity<Rating>().Property(x => x.Primarytitle).HasColumnName("primarytitle");
            modelBuilder.Entity<Rating>().Property(x => x.rating).HasColumnName("rating");

            modelBuilder.Entity<TitleGenre>().ToTable("title_genres");
            modelBuilder.Entity<TitleGenre>().HasKey(x => new { x.TitleId, x.Genre });
            modelBuilder.Entity<TitleGenre>().Property(x => x.TitleId).HasColumnName("title_ID");
            modelBuilder.Entity<TitleGenre>().Property(x => x.Genre).HasColumnName("genre");

            modelBuilder.Entity<Person>().ToTable("persons");
            modelBuilder.Entity<Person>().HasKey(x => x.PersonId);
            modelBuilder.Entity<Person>().Property(x => x.PersonId).HasColumnName("person_ID");
            modelBuilder.Entity<Person>().Property(x => x.Name).HasColumnName("name");
            modelBuilder.Entity<Person>().Property(x => x.BirthYear).HasColumnName("birthyear");
            modelBuilder.Entity<Person>().Property(x => x.DeathYear).HasColumnName("deathyear");

         




        }
    }
}
