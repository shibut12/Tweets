using Microsoft.AspNetCore.Authentication.Cookies;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Tweets.Web.Models;
using Tweets.Web.Services;
using Xunit;

namespace Tweets.Web.Tests
{
    public class SutTwitterService
    {
        [Fact]
        public void MapJsonShouldParseTwitterfeedJson()
        {
            var jsonString = "[{\"created_at\": \""+DateTime.Now.ToString("ddd MMM dd HH:mm:ss K yyyy") +"\",\"full_text\": \"A Trailblazer is someone who's\",\"retweet_count\":\"10\",\"user\": {\"name\": \"Test\",\"screen_name\": \"Testuser\",\"profile_image_url_https\": \"https://test.com\"},\"entities\": {\"media\": [{\"media_url_https\": \"https://test.com\"}]}}]";

            var twitterServiceOptions = new TwitterServiceOptions(){};
            var mockedHttpHelper = new Mock<IHttpHelper>();
            mockedHttpHelper.Setup(fn => fn.GetTwitterFeed(It.IsAny<HttpClient>(), It.IsAny<string>())).ReturnsAsync(jsonString);

            var twitterService = new TwitterService(twitterServiceOptions, mockedHttpHelper.Object);
            var response = twitterService.MapJson(jsonString);

            Assert.IsType<List<Tweet>>(response);
        }

        [Theory]
        [InlineData("salesforce")]
        [InlineData("bbcnews")]
        [InlineData("shibut12")]
        public async void GetTweetsJsonShouldReturnTwitterfeedJson(string username)
        {
            var jsonString = "[{\"created_at\": \"" + DateTime.Now.ToString("ddd MMM dd HH:mm:ss K yyyy") + "\",\"full_text\": \"A Trailblazer is someone who's\",\"retweet_count\":\"10\",\"user\": {\"name\": \"Test\",\"screen_name\": \"Testuser\",\"profile_image_url_https\": \"https://test.com\"},\"entities\": {\"media\": [{\"media_url_https\": \"https://test.com\"}]}}]";

            var twitterServiceOptions = new TwitterServiceOptions()
            {
                oauth_consumer_key = "CONSUMER_KEY",
                oauth_consumer_secret = "CONSUMER_SECRET",
                oauth_token = "ACCESS_TOKEN",
                oauth_token_secret = "ACCESS_TOKEN_SECRET"
            };

            var mockedHttpHelper = new Mock<IHttpHelper>();
            mockedHttpHelper.Setup(fn => fn.GetTwitterFeed(It.IsAny<HttpClient>(), It.IsAny<string>())).ReturnsAsync(jsonString);

            var twitterService = new TwitterService(twitterServiceOptions, mockedHttpHelper.Object);
            var response = await twitterService.GetTweetsJson("salesforce");

            Assert.IsType<string>(response);
            Assert.Equal(jsonString, response);
        }
    }
}
