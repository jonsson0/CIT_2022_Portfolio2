using System;
using System.Text.Json.Serialization;

namespace DataLayer.Models
{
    public class TitleGenre
    {
        public string TitleId { get; set; }
     
        
        /*
        [JsonIgnore]
        public Title? Title { get; set; }
        */

        public string? Genre { get; set; }
    }
}
