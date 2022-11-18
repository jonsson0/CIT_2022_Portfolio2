using System.Net;
using System.Text;
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

        //[Fact]
        //public void ApiUsersUpdateUserPassword()
        //{
        //    var updatepassword = PutData($"{UsersApi}/testing123/updatepassword/12345/12345", "12345");
        //    var (user, statusCode) = GetObject($"{UsersApi}/testing123");

        //    Assert.Equal(HttpStatusCode.OK, updatepassword);
        //    Assert.Equal("12345", user["password"]);
        //}
        //[Fact]
        //public void ApiUsers_DeleteUser()
        //{
        //    var data = DeleteData($"{UsersApi}/testing123/delete/1234");

        //    Assert.Equal(HttpStatusCode.OK, data);
   
        //}

        //[Fact]
        //public void ApiUsers_PostRegisterUser()
        //{
        //    var (data, statusCode) = PostData($"{UsersApi}/register", "{testuser},{password}");

        //    Assert.Equal(HttpStatusCode.OK, statusCode);
        //}

        [Fact]
        public void ApiPersons_GetWithNoArguments_OkAndAllPersons()
        {
            var (person, statusCode) = GetObject($"{PersonsApi}");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(10, person["items"].Count());
            Assert.Equal(28282, person["pages"]);
            Assert.Equal(282820, person["total"]);
            Assert.Equal($"{PersonsApi}?page=1&pageSize=10", person["next"].ToString());
        }

        [Fact]
        public void ApiPersons_GetWithValidPersonId_OkAndPerson()
        {
            var (person, statusCode) = GetObject($"{PersonsApi}/nm0000001");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal("Fred Astaire", person["name"]);

        }

        [Fact]
        public void ApiPersons_GetCoActorsWithValidPersonId()
        {
            var (CoActors, statusCode) = GetArray($"{PersonsApi}/nm0000002/CoActors");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal(6, CoActors.Count());
            Assert.Equal("Kirk Douglas", CoActors.First["name"]);
            Assert.Equal("Paul Bettany", CoActors.Last["name"]);
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

        HttpStatusCode DeleteData(string url)
        {
            var client = new HttpClient();
            var response = client.DeleteAsync(url).Result;
            return response.StatusCode;
        }

        HttpStatusCode PutData(string url, object content)
        {
            var client = new HttpClient();
            var response = client.PutAsync(
                url,
                new StringContent(
                    JsonConvert.SerializeObject(content),
                    Encoding.UTF8,
                    "application/json")).Result;
            return response.StatusCode;
        }

        (JObject, HttpStatusCode) PostData(string url, object content)
        {
            var client = new HttpClient();
            var requestContent = new StringContent(
                JsonConvert.SerializeObject(content),
                Encoding.UTF8,
                "application/json");
            var response = client.PostAsync(url, requestContent).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }
    }
}