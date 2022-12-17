using System.Text.Json.Serialization;

namespace CIT_2022_Portfolio2.Models
{
    public class CharacterModel
    {
        public string Url { get; set; }
        public string PersonId { get; set; }
        public string Name { get; set; }
        public string TitleId { get; set; }
        public string TitleCharacter { get; set; }
    }
}
