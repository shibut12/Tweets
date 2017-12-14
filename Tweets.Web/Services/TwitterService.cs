using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tweets.Web.Models;

namespace Tweets.Web.Services
{
    public class TwitterService : ITwitterService
    {
        private TwitterServiceOptions _twitterServiceOptions;
        private IHttpHelper _httpHelper;

        public TwitterService(TwitterServiceOptions twitterServiceOptions, IHttpHelper httpHelper)
        {
            _twitterServiceOptions = twitterServiceOptions;
            _httpHelper = httpHelper;
        }

        public async Task<string> GetTweetsJson(string screenName)
        {
            var oauth_version = "1.0";
            var oauth_signature_method = "HMAC-SHA1";

            var oauth_nonce = Convert.ToBase64String(
                new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            var timeSpan = DateTime.UtcNow
                            - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();
            int tweetCount = 10;
 
            var resource_url = "https://api.twitter.com/1.1/statuses/user_timeline.json";

            var baseFormat = "count={0}&include_entities=true&oauth_consumer_key={1}&oauth_nonce={2}&oauth_signature_method={3}" +
                                "&oauth_timestamp={4}&oauth_token={5}&oauth_version={6}&screen_name={7}&tweet_mode=extended";

            var baseString = string.Format(baseFormat,
                tweetCount,
                _twitterServiceOptions.oauth_consumer_key,
                oauth_nonce,
                oauth_signature_method,
                oauth_timestamp,
                _twitterServiceOptions.oauth_token,
                oauth_version,
                Uri.EscapeDataString(screenName)
            );

            baseString = string.Concat("GET&", Uri.EscapeDataString(resource_url), "&", Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(_twitterServiceOptions.oauth_consumer_secret),
                "&", Uri.EscapeDataString(_twitterServiceOptions.oauth_token_secret));

            string oauth_signature;
            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                    hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                                "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                                "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                                "oauth_version=\"{6}\"";

            var authHeader = string.Format(headerFormat,
                Uri.EscapeDataString(oauth_nonce),
                Uri.EscapeDataString(oauth_signature_method),
                Uri.EscapeDataString(oauth_timestamp),
                Uri.EscapeDataString(_twitterServiceOptions.oauth_consumer_key),
                Uri.EscapeDataString(_twitterServiceOptions.oauth_token),
                Uri.EscapeDataString(oauth_signature),
                Uri.EscapeDataString(oauth_version)
            );

            var postBody = "screen_name=" + Uri.EscapeDataString(screenName) + "&include_entities=true&tweet_mode=extended&count="+tweetCount;
            resource_url += "?" + postBody;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", authHeader);
                string result = await _httpHelper.GetTwitterFeed(client, resource_url);
                return result;
            }
        }

        public List<Tweet> MapJson(string twitterResponse)
        {
            List<Tweet> mappedTweets = new List<Tweet>();
            dynamic tweets = JsonConvert.DeserializeObject(twitterResponse);

            foreach (var tweetItem in tweets)
            {
                Tweet tweet = new Tweet();
                if (tweetItem["full_text"] != null)
                    tweet.FullText = (string)tweetItem["full_text"];
                else
                    tweet.FullText = "";
                if (tweetItem["user"] != null)
                    tweet.UserName = (string)tweetItem["user"]["name"];
                else
                    tweet.UserName = "";

                tweet.ScreenName = (string)tweetItem["user"]["screen_name"];
                tweet.ProfileImageUrl = (string)tweetItem["user"]["profile_image_url_https"];
                if (tweetItem["entities"]["media"] != null)
                    tweet.MediaUrl = (string)tweetItem["entities"]["media"][0]["media_url_https"];
                else if (tweetItem["quoted_status"] != null && tweetItem["quoted_status"]["entities"]["media"] != null)
                {
                    tweet.MediaUrl = (string)tweetItem["quoted_status"]["entities"]["media"][0]["media_url_https"];
                }
                else
                    tweet.MediaUrl = "";

                if (tweetItem["retweet_count"] != null)
                    tweet.RetweetCount = (int)tweetItem["retweet_count"];
                else
                    tweetItem["retweet_count"] = "";

                tweet.TweetDate = DateTime.ParseExact((string)tweetItem["created_at"], "ddd MMM dd HH:mm:ss K yyyy", CultureInfo.InvariantCulture).ToString("D");
                mappedTweets.Add(tweet);
            }

            return mappedTweets;
        }
    }
}
