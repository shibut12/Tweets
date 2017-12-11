using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tweets.Web.Services
{
    public class HttpHelper : IHttpHelper
    {
        public async Task<string> GetTwitterFeed(HttpClient client, string uri)
        {
            using (var r = await client.GetAsync(new Uri(uri)))
            {
                string result = await r.Content.ReadAsStringAsync();
                return result;
            }
        }
    }
}
