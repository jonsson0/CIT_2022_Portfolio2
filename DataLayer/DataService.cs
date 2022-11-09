﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using DataLayer.DataTransferObjects;
using DataLayer.Models;
using DataLayer.Models.Test;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DataLayer
{
    public class DataService : IDataService
    {
        //ImdbContext db = new ImdbContext();


        // Titles:
        // get
        public TitleOnMainPageDTO getTitle(string id)
        {
            using var db = new ImdbContext();
            var title = db
                        .Titles
                        .Include(x => x.TitleGenres)
                        .Where(x => x.TitleId == id).ToList().First();
            var titleDTO = CreateTitleOnMainPageDTO(title);
            return titleDTO;
        }
        public List<TitleOnMainPageDTO> getTitles()
        {
            using var db = new ImdbContext();

            var titles = db.Titles.ToList().GetRange(0, 3);
            List<TitleOnMainPageDTO> titlesDTO = new List<TitleOnMainPageDTO>();
           
            foreach (var title in titles)
            {
                var titleDTO = CreateTitleOnMainPageDTO(title);
               titlesDTO.Add(titleDTO);
            }
            return titlesDTO;
        }

        public List<TitleGenre> getTitlesByGenre(string genre)
        {
            using var db = new ImdbContext();
            var list = db
                .TitleGenres
                .Include(x => x.Title)
                .Where(x => x.Genre == genre).ToList();
            return list;
        }
        public List<Similar_Title>? getSimilarTitles(string id)
        {
            using var db = new ImdbContext();
            var list = db.SimilarMovies.FromSqlInterpolated($"select * FROM similar_movies({id})");
            return list.ToList();
        }

        // Other
        public void insertTitle(Title title)
        {
            using var db = new ImdbContext();
            db.Titles.Add(title);
            db.SaveChanges();
        }

        // Persons:
        public Person getPerson(string id)
        {
            using var db = new ImdbContext();
            var person = db.Person.Find(id);
            return person;
        }

        public List<Person> getPerson()
        {
            using var db = new ImdbContext();
            return db.Person.ToList().GetRange(0, 3);
        }


        public Person createPerson(string personId, string name, string birthYear, string deathYear)
        {
            using var db = new ImdbContext();
            var person = new Person();
            person.PersonId = personId;
            person.Name = name;
            person.BirthYear = birthYear;
            person.DeathYear = deathYear;
            db.Add(person);
            db.SaveChanges();
            return person;
        }

        public Boolean deletePerson(string personId)
        {
            using var db = new ImdbContext();
            var c = db.Person.Find(personId);
            if (c != null)
            {
                db.Remove(c);
                db.SaveChanges();
                return true;
            }
            else { return false; }
        }
        public Boolean updatePerson(string personId, string name, string birthYear, string deathYear)
        {
            using var db = new ImdbContext();

            var conn = (NpgsqlConnection)db.Database.GetDbConnection();
            conn.Open();
            var cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"UPDATE persons SET deathyear = {deathYear}, birthyear = {birthYear}, name = '{name}' WHERE \"person_ID\" = '{personId}'";
            return cmd.ExecuteNonQuery() > 0;
            //var c = db.Person.Find(personId);

            //if (c != null)
            //{
            //    //c.Name = name;
            //    //c.BirthYear = birthYear;
            //    c.DeathYear = deathYear+10;
            //    db.SaveChanges();
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }





















        // Users:
        public Boolean createUser(string username, string password)
        {
            using var db = new ImdbContext();
            if (username == null || password == null)
            {
                return false;
            }
            else
            {
                var user = new User();
                user.Username = username;
                user.Password = password;
                db.Add(user);
                db.SaveChanges();
                return true;
            }
        }

        public Boolean updateUserPassword(string username, string oldpassword, string newpassword)
        {
            using var db = new ImdbContext();
            //var user = db.Users.FromSqlInterpolated($"select update_password({username, oldpassword, newpassword})");
            var user = db.Users.Find(username);
            if(user != null && oldpassword == user.Password)
            {
                user.Password = newpassword;
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean deleteUser(string username, string password)
        {
            using var db = new ImdbContext();
            var user = db.Users.Find(username);
            if(user != null && user.Password == password)
            {
                db.Remove(user);
                db.SaveChanges();
                return true;
            }
            else { return false; }
        }

        public Boolean createBookmarkPerson(string username, string personID)
        {
            using var db = new ImdbContext();
            var user = db.Users.Find(username);
            var person = db.Person.Find(personID);

            if (user != null && person != null)
            {
                var bookmark = new BookmarkPerson();
                bookmark.Persons.PersonId = personID;
                bookmark.User.Username = username;
                bookmark.Timestamp = new DateTime();
                return true;
            }
            else { return false; }
        }

        public Boolean createBookmarkTitle(string username, string titleID)
        {
            using var db = new ImdbContext();
            var user = db.Users.Find(username);
            var person = db.Titles.Find(titleID);

            if (user != null && titleID != null)
            {
                var bookmark = new BookmarkTitle();
                bookmark.Title.TitleId = titleID;
                bookmark.User.Username = username;
                bookmark.Timestamp = new DateTime();
                return true;
            }
            else { return false; }
        }

        // Skal have lavet mere på denne her så den opdatere total votes på titel, samt avg rating
        public Boolean createRating(string username, string titleID, float rating)
        {
            using var db = new ImdbContext();
            var user = db.Users.Find(username);
            var title = db.Titles.Find(titleID);

            if (user != null && title != null && rating <= 10 && rating >= 0)
            {
                var userrating = new Rating();
                userrating.User.Username = username;
                userrating.Title.TitleId = titleID;
                userrating.rating = rating;
                db.Add(userrating);
                db.SaveChanges();
                return true;
            }
            else { return false; }
        }



        // HELPERS

        public TitleOnMainPageDTO CreateTitleOnMainPageDTO(Title title)
        {

            var titleOnMainPageDTO = new TitleOnMainPageDTO
            {
                TitleId = title.TitleId,
                Type = title.Type,
                PrimaryTitle = title.PrimaryTitle,
                OriginalTitle = title.OriginalTitle,
                IsAdult = title.IsAdult,
                StartYear = title.StartYear,
                EndYear = title.EndYear,
                RunTimeMinutes = title.RunTimeMinutes,
                Poster = title.Poster,
                Plot = title.Plot,
                AverageRating = title.AverageRating,
                NumVotes = title.NumVotes,
                TitleGenreList = title.TitleGenres
            };
            return titleOnMainPageDTO;
        }

    }
}
