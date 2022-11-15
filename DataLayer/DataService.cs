using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using DataLayer.DataTransferObjects;
using DataLayer.Models;
using DataLayer.Models.ObjectsFromFunctions;
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
                                    //.ToList().GetRange(0, 3)
                                    .Take(3).ToList()
                                    .Select(x => createTitleOnMainPageDTO(x)).ToList();

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

        public List<Similar_Title>? getSimilarTitles(string id, int page, int pageSize)
        {
            using var db = new ImdbContext();
            var list = db.SimilarTitles.FromSqlInterpolated($"select * FROM similar_movies({id}) OFFSET {page*pageSize} LIMIT {pageSize}");

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

            var personOnMainPageDTO = createPersonOnMainPageDTO(person);
            return personOnMainPageDTO;
        }

        public PersonOnMainPageDTO getPersonName(string name)
        {
            using var db = new ImdbContext();
            var person = db
                .Persons
                .Find(name);

            var personOnMainPageDTO = createPersonOnMainPageDTO(person);
            return personOnMainPageDTO;
        }


        public List<PersonOnMainPageDTO> getPersons()
        {
            using var db = new ImdbContext();
            var persons = db
                .Persons
                .ToList()
                .Take(10).ToList()
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

        public List<CoActor_Person>? getCoActors(string name)
        {
            using var db = new ImdbContext();
            //var list = db.CoActorPerson.FromSqlInterpolated($"select * FROM searchCoActorsByName({name})");
            var list = db.CoActorPerson.FromSqlInterpolated($"select * FROM searchCoActorsByPersonId({name})");

            return list.ToList();
        }




















        // Users:

        public UserPageDTO getUser(string username)
        {
            using var db = new ImdbContext();
            
            var user = db.Users.Find(username);

            if (user != null)
            {
                var userpagedto = createUserPageDTO(user);
                return userpagedto;
            }
            else { return null; }
            
            
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

        public Boolean createBookmarkPerson(string username, string personname)
        {
            using var db = new ImdbContext();
            var user = db.Users.Find(username);
            var person = db.Persons.Where(x => x.Name == personname);
            
            if (user != null && person != null)
            {
                db.Database.ExecuteSqlInterpolated($"select input_bookmark_person({username},{personname})");
                Console.WriteLine("person booked");
                db.SaveChanges();
                //Console.WriteLine(test.ToList().FirstOrDefault());
                return true;
            }
            else { return false; }
        }

        public Boolean createBookmarkTitle(string username, string primarytitle)
        {
            using var db = new ImdbContext();
            var user = db.Users.Find(username);
            var title = db.Titles.Find(primarytitle);

            if (user != null && title != null)
            {
                db.Database.ExecuteSqlInterpolated($"select input_bookmark_person({username},{primarytitle})");
                db.SaveChanges();
                return true;
            }
            else { return false; }
        }

        public List<BookmarkPerson> getBookmarkPersonByUser(string username)
        {
            using var db = new ImdbContext();
            var user = db.Users.Find(username);

            if(user != null)
            {
                Console.WriteLine("User has bookmarked these actors: \n");
                var result = db.BookmarkPersons.FromSqlInterpolated($"select get_bookmark_person_by_user({username})");// WHERE bookmark_persons.username = '{username}'");
                return result.ToList();
                //foreach (var bookperson in result)
                //{
                //    Console.WriteLine(bookperson.Username + bookperson.Personname);
                //}
            }
            else { return null; }
        }

        public List<BookmarkTitle> getBookmarkTitleByUser(string username)
        {
            using var db = new ImdbContext();

            if (username != null)
            {
                Console.WriteLine("User has bookmarked these titles: \n");
                var result = db.BookmarkTitles.FromSqlInterpolated($"select get_bookmark_title_by_user({username})");
                return result.ToList();
            }
            else { return null; }
            //foreach (BookmarkTitle bookmarktitle in result)
            ////{
            ////    Console.WriteLine(bookmarktitle.Username
            ////        + bookmarktitle.Primarytitle);
            ////}
        }

        // Skal have lavet mere på denne her så den opdatere total votes på titel, samt avg rating
        public Boolean createRating(string username, string titlename, float rating)
        {
            using var db = new ImdbContext();
            var user = db.Users.Find(username);
            var title = db.Titles.Find(titlename);

            if (user != null && title != null)
            {
                var result = db.Ratings.FromSqlInterpolated($"select input_rating({username},{titlename},{rating})");
                return true;
            }
            else { return false; }
        }

        public List<Rating> getRatingsByUser (string username)
        {
            using var db = new ImdbContext();
            var user = db.Users.Find(username);

            if(user != null)
            {
                var result = db.Ratings.FromSqlInterpolated($"select get_ratings_by_user({username})");
                return result.ToList();
            }
            else { return null; }
        }



        // HELPERS

        public TitleOnMainPageDTO createTitleOnMainPageDTO(Title title)
        {
            using var db = new ImdbContext();

            // Could implement mapping

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

            // Could implement mapping

            var personOnMainPageDTO = new PersonOnMainPageDTO
            {
                PersonId = person.PersonId,
                Name = person.Name,
                BirthYear = person.BirthYear,
                DeathYear = person.DeathYear
            };
            return personOnMainPageDTO;
        }

        public UserPageDTO createUserPageDTO(User user)
        {
            using var db = new ImdbContext();

            var username = db.Users.Find(user).Username;
            var bookmarkedtitles = getBookmarkTitleByUser(username);
            var bookmarkedactors = getBookmarkPersonByUser(username);
            var userratings = getRatingsByUser(username);


            var userPageDTO = new UserPageDTO
            {
                Username = user.Username,
                BookmarkedActors = bookmarkedactors,
                BookmarkedTitles = bookmarkedtitles,
                UserRatings = userratings
            };
            return userPageDTO;
        }
    }
}