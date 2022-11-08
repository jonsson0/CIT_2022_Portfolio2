using DataLayer.Models;

namespace DataLayer
{
    public interface IDataService
    {
        // Titles
        Title getTitle(string id);

        public List<Title> getTitles();

        // Persons



        // Users

        public Boolean createUser(string username, string password);
        public Boolean updateUserPassword(string username, string oldpassword, string newpassword);
        public Boolean deleteUser(string username, string password);
    }
}