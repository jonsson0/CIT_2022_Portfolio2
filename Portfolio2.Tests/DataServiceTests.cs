
using CIT_2022_Portfolio2.Models;
using DataLayer;
using DataLayer.Models;

namespace Portfolio2.Tests
{
    public class DataServiceTests
    {
        // titles


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


        // User
        public void CreateCategory_ValidData_CreteCategoryAndReturnsNewObject()
        {
            var service = new DataService();
            var user = service.createUser("testCreateUser", "12345");
            Assert.True(user);
            Assert.Equal("testCreateUser", user.name);
            Assert.Equal("CreateCategory_ValidData_CreteCategoryAndReturnsNewObject", category.Description);

            // cleanup
            service.DeleteCategory(category.Id);
        }

    }
}