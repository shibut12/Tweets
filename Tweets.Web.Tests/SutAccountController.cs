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
            var accountController = new AccountController();
            var response = accountController.Login();
            Assert.IsType<ViewResult>(response);
        }
        [Fact]
        public void ExternalSigninShouldReturnAChallengeResultForTwitter()
        {
            var accountController = new AccountController();
            var response = accountController.ExternalSignin("Twitter");
            Assert.IsType<ChallengeResult>(response);
        }
    }
}
