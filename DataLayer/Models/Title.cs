﻿using System;
namespace DataLayer.Models
{
    public class Title
    {
        public char TitleId { get; set; }
        public string Type { get; set; }
        public string PrimaryTitle { get; set; }
        public string OriginalTitle { get; set; }
        public string IsAdult { get; set; }
        public char StartYear { get; set; }
        public char EndYear { get; set; }
        public int RunTimeMinutes { get; set; }
        public string Poster { get; set; }
        public object Plot { get; set; }
        public object MyProperty { get; set; }
        public double AverageRating { get; set; }
        public int NumVotes { get; set; }
    }
}

