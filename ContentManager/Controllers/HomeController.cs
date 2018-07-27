using Microsoft.AspNetCore.Mvc;

namespace ContentManager.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}