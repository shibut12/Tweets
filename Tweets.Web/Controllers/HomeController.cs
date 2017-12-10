using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using Tweets.Web.Models;
using Tweets.Web.Services;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

namespace Tweets.Web.Controllers
{
    public class HomeController:Controller
    {
        private ITwitterService _twitterService;

        public HomeController(ITwitterService twitterService)
        {
            _twitterService = twitterService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tweetsJson = await _twitterService.GetTweetsJson("salesforce");
            TwitterFeedViewModel model = new TwitterFeedViewModel();
            model.Tweets = _twitterService.MapJson(tweetsJson);
            

            return View();
        }
    }
}
