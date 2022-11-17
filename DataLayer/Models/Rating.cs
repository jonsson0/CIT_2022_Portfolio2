using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Rating
    {
        //public User User { get; set; }
        public string Username { get; set; }
        //public Title Title { get; set; }
        public string Title_ID { get; set; }
        public double rating { get; set; }
    }
}
