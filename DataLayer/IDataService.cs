using DataLayer.DataTransferObjects;
using DataLayer.Models;

namespace DataLayer
{
    public interface IDataService
    {
        // Titles
        // Get
        TitleOnMainPageDTO getTitle(string id);
        List<TitleOnMainPageDTO> getTitles();
        List<Title> getTitlesByGenre(TitleGenre genre);
        // List<Similar_Title>? getSimilarTitles(string id);
        // List<Person> getPersonsByTitle();

        List<Similar_Title>? getSimilarTitles(string id);


        // Other
        void insertTitle(Title title);

        // Persons

        public PersonOnMainPageDTO getPerson(string id);
        public List<PersonOnMainPageDTO> getPersons();
        public Person createPerson(string personId, string name, string birthYear, string deathYear);
        public Boolean deletePerson(string personId);
        public Boolean updatePerson(string personId, string name, string birthYear, string deathYear);



        // Users

        public Boolean createUser(string username, string password);
        public Boolean updateUserPassword(string username, string oldpassword, string newpassword);
        public Boolean deleteUser(string username, string password);

    }
}