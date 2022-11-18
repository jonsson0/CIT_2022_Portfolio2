using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit.Abstractions;
using static System.Net.WebRequestMethods;

namespace Portfolio2.Tests
{
    public class WebServiceTests
    {
        private const string PersonsApi = "http://localhost:5001/api/persons";
        private const string UsersApi = "http://localhost:5001/api/users";
        private const string TitlesApi = "http://localhost:5001/api/titles";

        // titles

        [Fact]
        public void ApiTitles_AllTitles_PagingOn()
        {
            var (TitlesJObject, statusCode) = GetObject($"{TitlesApi}");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(10, TitlesJObject["items"].Count());
            Assert.Equal(8223, TitlesJObject["pages"]);
            Assert.Equal(82227, TitlesJObject["total"]);
            Assert.Equal($"{TitlesApi}?page=1&pageSize=10", TitlesJObject["next"]);

        }

        [Fact]
        public void ApiTitles_ValidId()
        {
            var (title, statusCode) = GetObject($"{TitlesApi}/tt0052520");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal("The Twilight Zone", title["primaryTitle"]);
        }

        [Fact]
        public void ApiTitles_InvalidId()
        {
            var (title, statusCode) = GetObject($"{TitlesApi}/asokf");

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public void Api_InvalidPath()
        {
            var (title, statusCode) = GetObject($"http://localhost:5001/api/ti");

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public void ApiTitles_TitleValidId_ListOfSimilarTitles()
        {
            var (similarTitlesJObject, statusCode) = GetObject($"{TitlesApi}/tt0052520/similartitles");
            var similarTitlesList = similarTitlesJObject["items"];


            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(10, similarTitlesList.Count());
            Assert.Equal("Episode #1.13", similarTitlesList.First()["primaryTitle"].ToString());
            Assert.Equal("tt14735122", similarTitlesList.First()["titleId"].ToString());
            Assert.Equal("Doppelgänger III: Deja Vu", similarTitlesList.Last()["primaryTitle"].ToString());
        }


        [Fact]
        public void ApiUsers_GetWithNoArguments_OkAndAllUsers()
        {
            var (data, statusCode) = GetArray(UsersApi);

            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void ApiUsers_GetWithValidUserID_OkAndUser()
        {
            var (user, statusCode) = GetObject($"{UsersApi}/testing123");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal("testing123", user["username"]);
        }

        [Fact]
        public void ApiPersons_GetWithNoArguments_OkAndAllPersons()
        {
            var (data, statusCode) = GetArray(PersonsApi);

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(10, data.Count);
            Assert.Equal("Fred Astaire", data.First()["name"]);
            Assert.Equal("Olivia de Havilland", data.Last()["name"]);
        }

        [Fact]
        public void ApiPersons_GetWithValidPersonId_OkAndPerson()
        {
            var (person, statusCode) = GetObject($"{PersonsApi}/nm0000001"); // bør trim PersonId til 1

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal("Fred Astaire", person["name"]);
        }

        // Helpers

        (JArray, HttpStatusCode) GetArray(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JArray)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        (JObject, HttpStatusCode) GetObject(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }
    }
}