using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataTransferObjects
{
    public class TitleSearchInListDTO
    {
        public string TitleId { get; set; }
        public string Type { get; set; }
        public string PrimaryTitle { get; set; }
        public string OriginalTitle { get; set; }
        public bool IsAdult { get; set; }
        public string? StartYear { get; set; }
        public string? EndYear { get; set; }
        public int? RunTimeMinutes { get; set; }
        public string? Poster { get; set; }
        public string? Plot { get; set; }
        public double? AverageRating { get; set; }
        public int? NumVotes { get; set; }
        public List<TitleGenre>? TitleGenres { get; set; }
        public List<Character> TitleCharacters { get; set; }
    }
}
