using Microsoft.AspNetCore.Mvc;

namespace ContentManager.Controllers
{
    [Route("/dashtest")]
    public class DashTestController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}