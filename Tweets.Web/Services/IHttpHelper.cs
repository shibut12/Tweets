using System.Net.Http;
using System.Threading.Tasks;

namespace Tweets.Web.Services
{
    public interface IHttpHelper
    {
        Task<string> GetTwitterFeed(HttpClient client, string uri);
    }
}
