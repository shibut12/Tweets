using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweets.Web.Models;
using Tweets.Web.Services;

namespace Tweets.Web.Controllers
{
    [Authorize]
    public class TwitterFeedController:Controller
    {
        private ITwitterService _twitterService;

        public TwitterFeedController(ITwitterService twitterService)
        {
            _twitterService = twitterService;
        }
        
        [HttpGet]
        public async Task<IEnumerable<Tweet>> GetAll()
        {
            var tweetsJson = await _twitterService.GetTweetsJson("salesforce");
            return (_twitterService.MapJson(tweetsJson));
        }
    }
}
