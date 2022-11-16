using System;
namespace DataLayer.Models
{
	public class PersonProfession
	{

		public string TitleId { get; set; }
		public string PersonId { get; set; }
		public string Category { get; set; }
		public string Job { get; set; }
		public Person Person { get; set; }
	}
}

