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
<<<<<<< Updated upstream
        public string PersonName { get; set; }
        public Person Persons { get; set; }
=======
        public User Username { get; set; }
        public Person Persons { get; set; }
        public Person Name { get; set; }
>>>>>>> Stashed changes
        public DateTime Timestamp { get; set; }
    }
}
