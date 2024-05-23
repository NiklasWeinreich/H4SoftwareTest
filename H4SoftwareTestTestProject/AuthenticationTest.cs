using Bunit;
using Bunit.TestDoubles;
using H4SoftwareTest.Components.Pages;
using System.Runtime.InteropServices;

namespace H4SoftwareTestTestProject
{
    public class AuthenticationTest
    {
        [Fact]
        public void AuthenticationView()
        {
            //Arrange
            var ctx = new TestContext();
            var authContext = ctx.AddTestAuthorization();
            authContext.SetAuthorized("");

            //Act
            var cut = ctx.RenderComponent<Home>();

            //Assert
            cut.MarkupMatches("<h1>Hello, world!</h1>\r\n<br />\r\n<div>\r\n    Hello Again\r\n</div>");
        }

        [Fact]
        public void AuthenticationCode()
        {
            //Arrange
            var ctx = new TestContext();
            var authContext = ctx.AddTestAuthorization();
            authContext.SetAuthorized("");

            //Act
            var cut = ctx.RenderComponent<Home>();
            var homeObj = cut.Instance;

            //Assert
            Assert.Equal(homeObj._isAuthenticated, true);
        }
    }
}