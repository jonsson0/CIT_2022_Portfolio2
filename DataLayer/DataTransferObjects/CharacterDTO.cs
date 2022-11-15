using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataLayer.DataTransferObjects
{
    public class CharacterDTO
    {
        public int CharacterId { get; set; }
        public string PersonId { get; set; }
        public string TitleId { get; set; }
        public string TitleCharacter { get; set; }
    }
}
