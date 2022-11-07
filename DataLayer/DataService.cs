using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;

namespace DataLayer
{
    public class DataService : IDataService
    {
        ImdbContext db = new ImdbContext();

        public Title getTItle(int id)
        {
            var title = db.Titles.Find(id);
            return title;
        }

        public List<Title> getTitles()
        {
            return db.Titles.ToList().GetRange(0, 3);
        }
    }
}
