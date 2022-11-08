using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using DataLayer.Models;
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
        public List<Title>? getSimilarTitles(string id)
        {
            var list = db.Titles.FromSqlInterpolated($"select similar_movies({id})");
            return list.ToList();
        }

        // Other
        public void insertTitle(Title title)
        {
            db.Titles.Add(title);
            db.SaveChanges();
        }



        // Persons:
        public Persons getPerson(string id)
        {
            var person = db.Persons.Find(id);
            return person;
        }

        public List<Persons> getPerson()
        {
            return db.Persons.ToList().GetRange(0, 3);
        }

        // Users:


        // HELPER MEHTODS


        /*
        public IQueryable Databaseset callDbFunction(string function)
        {
            var list = null;

            return list
        }
        */

    }
}
