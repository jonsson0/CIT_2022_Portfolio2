using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class BookmarkPerson
    {
        public string UserName { get; set; }
        public User User { get; set; }
        public string PersonName { get; set; }
        public Persons Persons { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
