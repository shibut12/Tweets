using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Tweets.Web.Controllers
{
    public class HomeController:Controller
    {
        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.User.Identity.Name; 
            return View();
        }
    }
}
