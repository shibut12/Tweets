using Moq;
using System.Collections.Generic;
using Tweets.Web.Controllers;
using Tweets.Web.Models;
using Tweets.Web.Services;
using Xunit;

namespace Tweets.Web.Tests
{
    public class SutTwitterFeedController
    {
        [Fact]
        public async void GetAllShouldReturnAlistOfTweet()
        {
            var tweetList = new List<Tweet>()
            {
                new Tweet()
                {
                    FullText = "Text of Tweet1"
                },
                new Tweet()
                {
                    FullText = "Text of Tweet2"
                },
                new Tweet()
                {
                    FullText = "Text of Tweet3"
                }
            };
            var mockedTwitterService = new Mock<ITwitterService>();
            mockedTwitterService.Setup(fn => fn.GetTweetsJson(It.IsAny<string>())).ReturnsAsync("{\"SAMPLEJSONELEMENT\":\"SAMPLEJSONVALUE\"}");
            mockedTwitterService.Setup(fn => fn.MapJson(It.IsAny<string>())).Returns(tweetList);

            var twitterFeed = new TwitterFeedController(mockedTwitterService.Object);
            var response = await twitterFeed.GetAll();
            Assert.Equal(tweetList, response);
        }
    }
}
