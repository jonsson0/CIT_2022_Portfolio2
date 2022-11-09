using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using DataLayer.Models;
using DataLayer.Models.Test;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DataLayer
{
    public class DataService : IDataService
    {
        ImdbContext db = new ImdbContext();


        // Titles:
        // get
        public Title getTitle(string id)
        {
            var title = db.Titles.Find(id);
            return title;
        }
        public List<Title> getTitles()
        {
            return db.Titles.ToList().GetRange(0, 3);
        }

        public List<TitleGenre> getTitlesByGenre(string genre)
        {
            var list = db
                .TitleGenres
                .Include(x => x.Title)
                .Where(x => x.Genre == genre).ToList();
            return list;
        }
        public List<Similar_Title>? getSimilarTitles(string id)
        {
            var list = db.SimilarMovies.FromSqlInterpolated($"select * FROM similar_movies({id})");
            return list.ToList();
        }

        // Other
        public void insertTitle(Title title)
        {
            db.Titles.Add(title);
            db.SaveChanges();
        }



        // Persons:
        public Person getPerson(string id)
        {
            var person = db.Person.Find(id);
            return person;
        }

        public List<Person> getPerson()
        {
            return db.Person.ToList().GetRange(0, 3);
        }


        public Person createPerson(String personId, string name, string birthYear, string deathYear)
        {
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
            var c = db.Person.Find(personId);
            if (c != null)
            {
                db.Remove(c);
                db.SaveChanges();
                return true;
            }
            else { return false; }
        }





















        // Users:
        public Boolean createUser(string username, string password)
        {
            if(username == null || password == null)
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
            var user = db.Users.Find(username);
            var person = db.Persons.Find(personID);

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

    }
}
