using System;
using System.Text.Json.Serialization;

namespace DataLayer.Models
{
    public class Character
    {
        public int CharacterId { get; set; }
        public string PersonId { get; set; }
        public Person Person { get; set; }
        public string TitleId { get; set; }
      
        [JsonIgnore]
        public Title Title { get; set; }
        public string TitleCharacter { get; set; }
    }
}

