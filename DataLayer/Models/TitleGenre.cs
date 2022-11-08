using System;
namespace DataLayer.Models
{
    public class TitleGenre
    {
        public string TitleId { get; set; }
        public Title? Title { get; set; }
        public string? Genre { get; set; }
    }
}
