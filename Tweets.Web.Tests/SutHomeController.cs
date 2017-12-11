using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Tweets.Web.Controllers;
using Xunit;

namespace Tweets.Web.Tests
{
    public class SutHomeController
    {
        [Fact]
        public void IndexShouldReturnAView()
        {
            var home = new HomeController();
            var response = home.Index();
            Assert.IsType<ViewResult>(response);
        }
    }
}
