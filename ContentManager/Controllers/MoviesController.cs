using Microsoft.AspNetCore.Mvc;

namespace ContentManager.Controllers
{
    public class MoviesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}