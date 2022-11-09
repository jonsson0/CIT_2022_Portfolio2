using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class BookmarkPerson
    {
        public User User { get; set; }
        public User Username { get; set; }
        public Persons Persons { get; set; }
        public Persons Name { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
