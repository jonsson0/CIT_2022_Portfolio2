using System;
namespace DataLayer.DataTransferObjects
{
	public class PersonsSearchInListDTO
    {

		public string PersonId { get; set; }
		public string Name { get; set; }
        public string? BirthYear { get; set; }
        public string? DeathYear { get; set; }

        //public IList<string> Jobs { get; set; }
    }
}

