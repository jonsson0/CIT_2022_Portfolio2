using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;

namespace DataLayer.DataTransferObjects
{
    public class TitleOnMainPage
    {
        public string PrimaryTitle { get; set; }
        public string Type { get; set; }
        public string IsAdult { get; set; }
        public char StartYear { get; set; }
        public char EndYear { get; set; }
        public int RunTimeMinutes { get; set; }
        public string Poster { get; set; }
        public string Plot { get; set; }
        public double AverageRating { get; set; }
        public int NumVotes { get; set; }
        List<TitleGenre>? TitleGenreList { get; set; }
    }
}
