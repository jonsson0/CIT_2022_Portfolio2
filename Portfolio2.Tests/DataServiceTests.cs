
using System.Xml.Linq;
using CIT_2022_Portfolio2.Models;
using DataLayer;
using DataLayer.Models;

namespace Portfolio2.Tests
{
    public class DataServiceTests
    {
        // titles
        [Fact]
        public void GetTitles_WithPaging()
        {
            var service = new DataService();
            var page = 0;
            var pageSize = 25;
            var titles = service.getTitles(page, pageSize);
            Assert.Equal(25, titles.Count);
            Assert.Equal("The Twilight Zone", titles.First().PrimaryTitle);
            Assert.Equal("tvSeries", titles.First().Type);
        }

        [Fact]
        public void GetTitle_ValidId_ReturnsTitle()
        {
            var service = new DataService();
            var title = service.getTitle("tt0052520");
            Assert.Equal("The Twilight Zone", title.PrimaryTitle);
            Assert.Equal("tvSeries", title.Type);
        }

        [Fact]
        public void GetSimilarTitlesToTitleWithPaging()
        {
            var service = new DataService();
            var titles = service.getSimilarTitles("tt0052520", 0, 25);
            Assert.Equal(25, titles.Count);
            Assert.Equal("Episode #1.13", titles.First().PrimaryTitle);
        }

        [Fact]
        public void GetCharactersByTitleWithPerson()
        {
            var service = new DataService();
            Title title = new Title
            {
                TitleId = "tt9999999",
                Type = "hey"
            };

        public string
            PrimaryTitle { get; set; }

        public string OriginalTitle { get; set; }
        public bool IsAdult { get; set; }

        var characters = service.getCharactersByTitle(title);
        Assert.Equal("John Anderson", title.TitleCharacters.First().Person.Name);
    





// Persons
        [Fact]
        public void Person_Object_HasPersonIdNameBirthYearDeathYear()
        {
            var person = new Person
            {
                PersonId = "1234",
                Name = "Jens",
                BirthYear = "1990",
                DeathYear = null
            };
         Assert.Equal("1234", person.PersonId);
         Assert.Equal("Jens", person.Name);
         Assert.Null(person.DeathYear);
        }

        [Fact]
        public void PersonModel_Object_HasPersonIdNameBirthYearDeathYear()
        {
            var personModel = new PersonModel();
            Assert.Null(personModel.url);
            Assert.Null(personModel.Name); // should not be null
            Assert.Null(personModel.BirthYear);
            Assert.Null(personModel.DeathYear);
        }

        [Fact]
        public void GetPersons_WithPaging()
        {
            var service = new DataService();
            var page = 0;
            var pageSize = 10;
            var persons = service.getPersons(page, pageSize);
            Assert.Equal(10, persons.Count);
            Assert.Equal("Fred Astaire", persons.First().Name);
            Assert.Equal("1899", persons.First().BirthYear);
            Assert.Equal("1987", persons.First().DeathYear);
        }

        [Fact]
        public void GetPerson_ValidId_ReturnsPerson()
        {
            var service = new DataService();
            var person = service.getPerson("nm0000002");
            Assert.Equal("Lauren Bacall", person.Name);
            Assert.Equal("1924", person.BirthYear);
            Assert.Equal("2014", person.DeathYear);
        }

        [Fact]
        public void getPersonByName_ReturnPerson()
        {
            var service = new DataService();
            var person = service.getPersonByName("Tom Hanks");
            Assert.Equal("nm0000158", person.First().PersonId);
            Assert.Equal("Tom Hanks", person.First().Name);
        }

        [Fact]
        public void CreateCategory_ValidData_CreteCategoryAndReturnsNewObject()
        {
            var service = new DataService();
            var person = service.createPerson("nm9993710", "Steen", "1991", null);
            var personFronDB = service.getPerson("nm9993710");
            Assert.Equal(person.PersonId, personFronDB.PersonId);
            Assert.Equal(person.BirthYear, personFronDB.BirthYear);
            Assert.Null(person.DeathYear);

            //// cleanup
            //service.DeleteCategory(category.Id);
        }

        // User
        /*public void CreateCategory_ValidData_CreteCategoryAndReturnsNewObject()
        {
            var service = new DataService();
            var user = service.createUser("testCreateUser", "12345");
            Assert.True(user);
            Assert.Equal("testCreateUser", user.name);
            Assert.Equal("CreateCategory_ValidData_CreteCategoryAndReturnsNewObject", category.Description);

            // cleanup
            service.DeleteCategory(category.Id);
        }*/

    }
}