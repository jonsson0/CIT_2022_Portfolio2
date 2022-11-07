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

    Title getTItle()
        {
            return null;
        }

        public List<Title> getTitles()
        {
            return db.Titles.ToList().GetRange(0,3);
        }



    }
}
