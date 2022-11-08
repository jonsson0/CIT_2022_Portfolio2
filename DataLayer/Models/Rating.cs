using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Rating
    {
        public User User { get; set; }
        public User Username { get; set; }
        public Title Title { get; set; }
        public Title Primarytitle { get; set; }
        public float rating { get; set; }
    }
}
