using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Portfolio2.Tests
{
    public class WebServiceTests
    {
        private const string PersonsApi = "http://localhost:5001/api/persons";

        [Fact]
        public void Test1()
        {

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