using System.Collections.Generic;
using System.Threading.Tasks;
using Tweets.Web.Models;

namespace Tweets.Web.Services
{
    public interface ITwitterService
    {
        Task<string> GetTweetsJson(string screenName);
        List<Tweet> MapJson(string twitterResponse);
    }
}
