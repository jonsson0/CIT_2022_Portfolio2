using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using DataLayer.DataTransferObjects;
using DataLayer.Models;
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
                //.Include(x => x.SimilarTitles)
                .Include(x => x.TitleCharacters)
                .ThenInclude(x => x.Person)
                //.Where(x => x.TitleCharacters)
                .FirstOrDefault(x => x.TitleId == id)
                ; //.Where(x => x. == false).ToList().First();

           

          //  title.TitleCharacters = getCharactersByTitle(title);

            if (title != null)
            {
                title.TitleCharacters = db.Characters.Where(x => x.TitleId == id).Take(10).ToList();
                var titleDTO = createTitleOnMainPageDTO(title);
                return titleDTO;
            }

            return null;
        }

        public List<Character> getCharactersByTitle(Title title)
        {
            using var db = new ImdbContext();

            var list = db
                .Characters
                .Include(x => x.Person)
                .Where(x => x.TitleId == title.TitleId)
                .ToList();
            return list;
        }


        public List<TitleOnMainPageDTO> getTitles()
        {
            using var db = new ImdbContext();

            var titles = db
                                    .Titles
                                    .Include(x => x.TitleGenres)
                                    .Select(x => createTitleOnMainPageDTO(x)).ToList();
            //.ToList().GetRange(0, 3);

          //  List<TitleOnMainPageDTO> titlesDTO = new List<TitleOnMainPageDTO>();
           
            
            //foreach (var title in titles)
            //{
            //    var titleDTO = CreateTitleOnMainPageDTO(title);
            //   titlesDTO.Add(titleDTO);
            //}
            

            return titles;
        }

        public List<Title> getTitlesByGenre(TitleGenre genre)
        {
            using var db = new ImdbContext();
            var list = db
                .Titles
                .Include(x => x.TitleGenres)
                .Where(x => x.TitleGenres.Contains(genre)).ToList();
            return list;
        }



        public List<Similar_Title>? getSimilarTitles(string id)
        {
            using var db = new ImdbContext();
            var list = db.SimilarTitles.FromSqlInterpolated($"select * FROM similar_movies({id})");

           // db.SimilarTitles.Where(x => x.TitleId == id);  //
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
        public PersonOnMainPageDTO getPerson(string id)
        {
            using var db = new ImdbContext();
            var person = db
                .Persons
                .Find(id);

            var dto = createPersonOnMainPageDTO(person);
            return dto;
        }

        public List<PersonOnMainPageDTO> getPersons()
        {
            using var db = new ImdbContext();
            var persons = db
                .Persons
                .Select(x => createPersonOnMainPageDTO(x)).ToList();
            return persons;
        }

        //    List<PersonOnMainPageDTO> DTOlist = new List<PersonOnMainPageDTO>();
        //    foreach(var person in persons)
        //    {
        //        var DTO = createPersonOnMainPageDTO(person);
        //        DTOlist.Add(DTO);
        //    }

        //    return DTOlist;
        //}




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
            var c = db.Persons.Find(personId);
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

            //var conn = (NpgsqlConnection)db.Database.GetDbConnection();
            //conn.Open();
            //var cmd = new NpgsqlCommand();
            //cmd.Connection = conn;
            //cmd.CommandText = $"UPDATE persons SET deathyear = {deathYear}, birthyear = {birthYear}, name = '{name}' WHERE \"person_ID\" = '{personId}'";
            //return cmd.ExecuteNonQuery() > 0;

            var c = db.Persons.Find(personId);

            if (c != null)
            {
                c.Name = name;
                c.BirthYear = birthYear;
                c.DeathYear = deathYear;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }





















        // Users:

        public string getUser(string username)
        {
            using var db = new ImdbContext();
            if (db.Users.Find(username) == null)
            {
                return "User does not exist";
            }
            else
            {
                var u_name = db.Users.Find(username).Username;
                var p_word = db.Users.Find(username).Password;
                return (u_name + "\n" + p_word);
            }
        }
        public Boolean createUser(string username, string password)
        {
            using var db = new ImdbContext();
            if (username == null || password == null)
            {
                return false;
            }
            else
            {
                db.Database.ExecuteSqlInterpolated($"select input_user({username},{password})");
                return true;
            }
        }

        public Boolean updateUserPassword(string username, string oldpassword, string newpassword)
        {
            using var db = new ImdbContext();
            var user = db.Users.Find(username);
            if (user != null && oldpassword == user.Password)
            {
                db.Database.ExecuteSqlInterpolated($"select update_password({username},{oldpassword},{newpassword})");
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
                db.Database.ExecuteSqlInterpolated($"select delete_user({username},{password})");
                return true;
            }
            else { return false; }
        }

        public Boolean createBookmarkPerson(string username, string personID)
        {
            using var db = new ImdbContext();
            var user = db.Users.Find(username);
            var person = db.Persons.Find(personID);

            if (user != null && person != null)
            {
                db.BookmarkPersons.FromSqlInterpolated($"select input_bookmark_person({username},{personID})");
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

        public void getBookmarkPersonByUser(string username)
        {
            using var db = new ImdbContext();
            var bookmarkpersonlist = db.BookmarkPersons.Where( x => x.UserName == username);
            Console.WriteLine("User has bookmarked these actors: \n");
            foreach(var bookmarkperson in bookmarkpersonlist)
            {
                Console.WriteLine(bookmarkperson.PersonName);
            }
            
        }

        public List<BookmarkTitle> getBookmarkTitleByUser(string username)
        {
            using var db = new ImdbContext();
            return db.BookmarkTitles.ToList();
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

        public static TitleOnMainPageDTO createTitleOnMainPageDTO(Title title)
        {
            using var db = new ImdbContext();

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
                TitleGenres = title.TitleGenres,
                TitleCharacters = title.TitleCharacters
                //  SimilarTitles =  title.SimilarTitles

            };
            return titleOnMainPageDTO;
        }

        public PersonOnMainPageDTO createPersonOnMainPageDTO(Person person)
        {
            using var db = new ImdbContext();

            var personOnMainPageDTO = new PersonOnMainPageDTO
            {
                PersonId = person.PersonId,
                Name = person.Name,
                BirthYear = person.BirthYear,
                DeathYear = person.DeathYear
            };
            return personOnMainPageDTO;
        }
    }
}