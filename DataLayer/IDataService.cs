using DataLayer.Models;
using DataLayer.Models.Test;

namespace DataLayer
{
    public interface IDataService
    {
        // Titles
        // Get
        Title getTitle(string id);
        List<Title> getTitles();
        List<TitleGenre> getTitlesByGenre(string genre);
        List<Similar_Title>? getSimilarTitles(string id);

        // Other
        void insertTitle(Title title);

        // Persons

        public Person getPerson(string id);
        public List<Person> getPerson();
        public Person createPerson(String personId, string name, string birthYear, string deathYear);
        public Boolean deletePerson(string personId);
        public Boolean updatePerson(String personId, string name, string birthYear, string deathYear);



        // Users

        public Boolean createUser(string username, string password);
        public Boolean updateUserPassword(string username, string oldpassword, string newpassword);
        public Boolean deleteUser(string username, string password);

    }
}