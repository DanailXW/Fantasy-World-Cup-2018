using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace FantasyCup.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
