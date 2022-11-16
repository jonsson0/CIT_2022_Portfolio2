using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;
using Microsoft.Extensions.Logging;
using DataLayer.Models.ObjectsFromFunctions;

namespace DataLayer
{
    public class ImdbContext : DbContext
    {

        const string ConnectionString = "host=cit.ruc.dk;db=cit09;uid=cit09;pwd=8wUBnJ0Lw4Zn";


        public DbSet<Title> Titles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<BookmarkPerson> BookmarkPersons { get; set; }
        public DbSet<BookmarkTitle> BookmarkTitles { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<TitleGenre> TitleGenres { get; set; }
        public DbSet<Similar_Title> SimilarTitles { get; set; }
        public DbSet<CoActor> CoActors { get; set; }

        public DbSet<Character> Characters { get; set; }
        public DbSet<PersonProfession> PersonProfessions { get; set; }


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
            modelBuilder.Entity<Title>().HasMany(x => x.TitleGenres); //.WithOne(x => x.Title);
                                                                      // modelBuilder.Entity<Title>().HasMany(x => x.SimilarTitles);
            modelBuilder.Entity<Title>().HasMany(x => x.TitleCharacters).WithOne(x => x.Title); //.WithOne(x => x.Title);

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
            // modelBuilder.Entity<Title>().ToTable("similar_movies");
            modelBuilder.Entity<Similar_Title>().HasNoKey();
            modelBuilder.Entity<Similar_Title>().Property(x => x.TitleId).HasColumnName("title_ID");
            modelBuilder.Entity<Similar_Title>().Property(x => x.PrimaryTitle).HasColumnName("primarytitle");
            

            // title_genre
            modelBuilder.Entity<TitleGenre>().ToTable("title_genres");
            modelBuilder.Entity<TitleGenre>().HasKey(x => new { x.TitleId, x.Genre });
            modelBuilder.Entity<TitleGenre>().Property(x => x.TitleId).HasColumnName("title_ID");
            modelBuilder.Entity<TitleGenre>().Property(x => x.Genre).HasColumnName("genre");


            // users
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<User>().HasKey(x => x.Username);
            modelBuilder.Entity<User>().Property(x => x.Username).HasColumnName("username");
            modelBuilder.Entity<User>().Property(x => x.Password).HasColumnName("password");

            modelBuilder.Entity<BookmarkPerson>().ToTable("bookmark_persons");
            modelBuilder.Entity<BookmarkPerson>().HasKey(x => new { x.Username, x.Personname });
            //modelBuilder.Entity<BookmarkPerson>().HasNoKey();
            modelBuilder.Entity<BookmarkPerson>().Property(x => x.Username).HasColumnName("username");
            modelBuilder.Entity<BookmarkPerson>().Property(x => x.Personname).HasColumnName("person_name");
            modelBuilder.Entity<BookmarkPerson>().Property(x => x.Timestamp).HasColumnName("person_timestamp");

            modelBuilder.Entity<BookmarkTitle>().ToTable("bookmark_titles");
            modelBuilder.Entity<BookmarkTitle>().HasKey(x => new { x.Username, x.Primarytitle });
            modelBuilder.Entity<BookmarkTitle>().Property(x => x.Username).HasColumnName("username");
            modelBuilder.Entity<BookmarkTitle>().Property(x => x.Primarytitle).HasColumnName("primary_title");
            modelBuilder.Entity<BookmarkTitle>().Property(x => x.Timestamp).HasColumnName("title_timestamp");

            modelBuilder.Entity<Rating>().ToTable("ratings");
            modelBuilder.Entity<Rating>().HasKey(x => new { x.Username, x.Primarytitle });
            modelBuilder.Entity<Rating>().Property(x => x.Username).HasColumnName("username");
            modelBuilder.Entity<Rating>().Property(x => x.Primarytitle).HasColumnName("primarytitle");
            modelBuilder.Entity<Rating>().Property(x => x.rating).HasColumnName("rating");

            // Persons
            modelBuilder.Entity<Person>().ToTable("persons");
            modelBuilder.Entity<Person>().HasKey(x => x.PersonId);
            modelBuilder.Entity<Person>().Property(x => x.PersonId).HasColumnName("person_ID");
            modelBuilder.Entity<Person>().Property(x => x.Name).HasColumnName("name");
            modelBuilder.Entity<Person>().Property(x => x.BirthYear).HasColumnName("birthyear");
            modelBuilder.Entity<Person>().Property(x => x.DeathYear).HasColumnName("deathyear");
            //modelBuilder.Entity<Person>().HasMany(x => x.PersonCharacters).WithMany(x => x.PersonId);
            modelBuilder.Entity<Person>().HasMany(x => x.PersonProfessions).WithOne(x => x.Person);


            // SearchCoActorById
            modelBuilder.Entity<CoActor>().HasNoKey();
            modelBuilder.Entity<CoActor>().Property(x => x.PersonId).HasColumnName("person_ID");
            modelBuilder.Entity<CoActor>().Property(x => x.Name).HasColumnName("name");
            modelBuilder.Entity<CoActor>().Property(x => x.Frequency).HasColumnName("frequency");


            // Characters
            modelBuilder.Entity<Character>().ToTable("characters");
            modelBuilder.Entity<Character>().HasKey(x => x.CharacterId);
            modelBuilder.Entity<Character>().Property(x => x.CharacterId).HasColumnName("character_ID");
            modelBuilder.Entity<Character>().Property(x => x.PersonId).HasColumnName("person_ID");
            modelBuilder.Entity<Character>().Property(x => x.TitleId).HasColumnName("title_ID");
            modelBuilder.Entity<Character>().Property(x => x.TitleCharacter).HasColumnName("character");

            // PersonProfessions
            modelBuilder.Entity<PersonProfession>().ToTable("person_professions");
            modelBuilder.Entity<PersonProfession>().HasKey(x => new { x.TitleId, x.PersonId, x.Category });
            modelBuilder.Entity<PersonProfession>().Property(x => x.TitleId).HasColumnName("title_ID");
            modelBuilder.Entity<PersonProfession>().Property(x => x.PersonId).HasColumnName("person_ID");
            modelBuilder.Entity<PersonProfession>().Property(x => x.Category).HasColumnName("category");
            modelBuilder.Entity<PersonProfession>().Property(x => x.Job).HasColumnName("job");
        }
    }
}
