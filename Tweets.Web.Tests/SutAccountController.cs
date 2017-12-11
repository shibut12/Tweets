using Xunit;
using Tweets.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Tweets.Web.Tests
{
    public class SutAccountController
    {
        [Fact]
        public void LoginShouldReturnAView()
        {
            var account = new AccountController();
            var response = account.Login();
            Assert.IsType<ViewResult>(response);
        }
        [Fact]
        public void ExternalSigninShouldReturnAChallengeResultForTwitter()
        {
            var account = new AccountController();
            var response = account.ExternalSignin("Twitter");
            Assert.IsType<ChallengeResult>(response);
        }
    }
}
