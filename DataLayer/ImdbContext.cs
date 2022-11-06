using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;

namespace DataLayer
{
    public class ImdbContext : DbContext
    {

        const string ConnectionString = "host=localhost;db=imdb;uid=postgres;pwd=1234"; // needs changing

        public DbSet<Title> Titles { get; set; }


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
        }
    }
}
