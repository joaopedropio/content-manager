using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ContentManager.Controllers
{
    [Route("/websocket")]
    public class WebSocketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
