using DataLayer.DataTransferObjects;
using DataLayer.Models;
using DataLayer.Models.ObjectsFromFunctions;

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
        public List<CoActor_Person> getCoActors(string id)




        // Users

        public UserPageDTO getUser(string username);
        public Boolean createUser(string username, string password);
        public Boolean updateUserPassword(string username, string oldpassword, string newpassword);
        public Boolean deleteUser(string username, string password);
        public Boolean createBookmarkPerson(string username, string personname);
        public Boolean createBookmarkTitle(string username, string primarytitle);
        public List<BookmarkPerson> getBookmarkPersonByUser(string username);
        public List<BookmarkTitle> getBookmarkTitleByUser(string username);
        public Boolean createRating(string username, string titleID, float rating);
        public List<Rating> getRatingsByUser(string username);





    }
}