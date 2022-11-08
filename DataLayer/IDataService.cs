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
    }
}