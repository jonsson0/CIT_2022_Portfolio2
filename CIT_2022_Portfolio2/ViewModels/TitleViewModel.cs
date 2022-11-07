﻿using DataLayer.Models;

namespace CIT_2022_Portfolio2.ViewModels
{
    public class TitleViewModel
    {
        public string Type { get; set; }
        public string PrimaryTitle { get; set; }
        public string OriginalTitle { get; set; }
        public bool IsAdult { get; set; }
        public string? StartYear { get; set; }
        public string? EndYear { get; set; }
        public int? RunTimeMinutes { get; set; }
        public string? Poster { get; set; }
        public string Plot { get; set; }
        public double? AverageRating { get; set; }
        public int? NumVotes { get; set; }
        List<TitleGenre> GenreList { get; set; }
    }
}
