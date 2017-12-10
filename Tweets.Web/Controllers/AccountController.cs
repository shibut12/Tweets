using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Tweets.Web.Controllers
{
    public class AccountController:Controller
    {
        public IActionResult Login()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult ExternalSignin(string provider)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/Account/ExternalLoginCallback"
            };
            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback()
        {
            //Extract info from externa; login
            var result = await HttpContext.AuthenticateAsync();
            //If authenticate failed, send them back to login page
            if (result?.Succeeded != true)
            {
                return RedirectToAction("Index", "Home");
            }

            var externalUser = result.Principal;
            var claims = externalUser.Claims.ToList();

            //If user signed in sussessfully, redirect user to the feed
            return RedirectToAction("Index", "TwitterFeed");
        }
    }
}
