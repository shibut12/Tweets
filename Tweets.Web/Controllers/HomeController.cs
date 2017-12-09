using Microsoft.AspNetCore.Mvc;

namespace Tweets.Web.Controllers
{
    public class HomeController:Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
