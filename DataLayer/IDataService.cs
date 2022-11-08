using DataLayer.Models;

namespace DataLayer
{
    public interface IDataService
    {
        // Titles
        // Get
        Title getTitle(string id);
        List<Title> getTitles();
        List<TitleGenre> getTitlesByGenre(string genre);
        List<Title>? getSimilarTitles(string id);

        // Other
        void insertTitle(Title title);

        // Persons



        // Users

        public Boolean createUser(string username, string password);
        public Boolean updateUserPassword(string username, string oldpassword, string newpassword);
        public Boolean deleteUser(string username, string password);
    }
}