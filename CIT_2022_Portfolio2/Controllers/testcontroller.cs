using Microsoft.AspNetCore.Mvc;

namespace CIT_2022_Portfolio2.Controllers
{
    public class testcontroller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
