using DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace CIT_2022_Portfolio2.Controllers
{
    [Route("api/Landing")]
    [ApiController]
    public class LandingController : Controller
    {
        private IDataService _dataService;

        [HttpGet(Name = "Landing")]
        //[Authorize]
        public IActionResult getUsers()
        {
            List<string> list = new List<string>();
            list.Add("http://localhost:5001/api/titles");
            list.Add("http://localhost:5001/api/persons");
            return Ok(list);
        }

    }
}
