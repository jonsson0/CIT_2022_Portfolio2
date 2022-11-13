using DataLayer.Models;

namespace CIT_2022_Portfolio2.Models
{
    public class UserModel
    {
        public string? url { get; set; }
        public string Username { get; set; }
        public List<BookmarkPerson>? BookmarkedActors { get; set; }
        public List<BookmarkTitle>? BookmarkedTitles { get; set; }
        public List<Rating>? UserRatings { get; set; }

    }
}
