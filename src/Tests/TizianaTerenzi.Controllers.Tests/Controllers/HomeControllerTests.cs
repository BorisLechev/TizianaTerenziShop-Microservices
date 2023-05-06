namespace TizianaTerenzi.Controllers.Tests.Controllers
{
    using MyTested.AspNetCore.Mvc;
    using TizianaTerenzi.WebClient.Controllers;
    using Xunit;

    public class HomeControllerTests
    {
        [Fact]
        public void IndexShouldReturnView()
        {
            MyController<HomeController>
                .Instance()
                .Calling(c => c.Index())
                .ShouldReturn()
                .View();
        }

        [Fact]
        public void PrivacyShouldReturnView()
        {
            MyController<HomeController>
                .Instance()
                .Calling(c => c.Privacy())
                .ShouldReturn()
                .View();
        }

        [Fact]
        public void TermsShouldReturnView()
        {
            MyController<HomeController>
                .Instance()
                .Calling(c => c.Terms())
                .ShouldReturn()
                .View();
        }
    }
}
