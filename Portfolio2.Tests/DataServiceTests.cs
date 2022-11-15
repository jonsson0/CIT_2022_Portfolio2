
using CIT_2022_Portfolio2.Models;
using DataLayer.Models;

namespace Portfolio2.Tests
{
    public class DataServiceTests
    {
        [Fact]
        public void Person_Object_HasPersonIdNameBirthYearDeathYear()
        {
            var person = new Person();
            Assert.Null(person.PersonId); // should not be null
            Assert.Null(person.Name); // should not be null
            Assert.Null(person.BirthYear);
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
    }
}