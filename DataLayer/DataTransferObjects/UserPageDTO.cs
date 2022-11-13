using DataLayer.Models;

namespace DataLayer.DataTransferObjects
{
    public class UserPageDTO
    {
        public string Username { get; set; }
        public List<BookmarkPerson> BookmarkedActors { get; set; }
        public List<BookmarkTitle> BookmarkedTitles { get; set; }
        public List<Rating> UserRatings { get; set; }
    }
}
