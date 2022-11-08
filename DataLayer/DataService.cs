using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
        public Boolean createUser(string username, string password)
        {
            if(username == null || password == null)
            {
                return false;
            }
            else
            {
                var user = new User();
                user.Username = username;
                user.Password = password;
                db.Add(user);
                db.SaveChanges();
                return true;
            }
        }

        public Boolean updateUserPassword(string username, string oldpassword, string newpassword)
        {
            var user = db.Users.Find(username);
            if(user != null && oldpassword == user.Password)
            {
                user.Password = newpassword;
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean deleteUser(string username, string password)
        {
            var user = db.Users.Find(username);
            if(user != null && user.Password == password)
            {
                db.Remove(user);
                db.SaveChanges();
                return true;
            }
            else { return false; }
        }

        public Boolean createBookmarkPerson(string username, string name)
        {
            var user = db.Users.Find(username);
            var person =
        }

    }
}
