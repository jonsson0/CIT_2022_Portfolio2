using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using DataLayer.Models;

namespace DataLayer
{
    public class DataService : IDataService
    {
        ImdbContext db = new ImdbContext();

        
        // Titles:
        public Title getTitle(string id)
        {
            var title = db.Titles.Find(id);
            return title;
        }

        public List<Title> getTitles()
        {
            return db.Titles.ToList().GetRange(0, 3);
        }

        // Persons:


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
