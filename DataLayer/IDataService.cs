using DataLayer.DataTransferObjects;
using DataLayer.Models;
using DataLayer.Models.Test;

namespace DataLayer
{
    public interface IDataService
    {
        // Titles
        // Get
        TitleOnMainPage getTitle(string id);
        List<TitleOnMainPage> getTitles();
        List<TitleGenre> getTitlesByGenre(string genre);
        List<Similar_Title>? getSimilarTitles(string id);

        // Other
        void insertTitle(Title title);

        // Persons

        public Person getPerson(string id);
        public List<Person> getPerson();
        public Person createPerson(string personId, string name, string birthYear, string deathYear);
        public Boolean deletePerson(string personId);
        public Boolean updatePerson(string personId, string name, string birthYear, string deathYear);



        // Users

        public Boolean createUser(string username, string password);
        public Boolean updateUserPassword(string username, string oldpassword, string newpassword);
        public Boolean deleteUser(string username, string password);

    }
}