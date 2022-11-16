using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class BookmarkPerson
    {
        // In later portfolio, change type, so the bookmarks contain types user and person
        public string Username { get; set; }
        //public User User { get; set; }
        public string Person_ID { get; set; }
        //public Person Person { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
