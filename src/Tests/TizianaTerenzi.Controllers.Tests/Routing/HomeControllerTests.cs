namespace TizianaTerenzi.Controllers.Tests.Routing
{
    using MyTested.AspNetCore.Mvc;
    using TizianaTerenzi.WebClient.Controllers;
    using Xunit;

    public class HomeControllerTests
    {
        [Fact]
        public void IndexRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/")
                .To<HomeController>(c => c.Index());
        }

        [Fact]
        public void PrivacyRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/home/privacy")
                .To<HomeController>(c => c.Privacy());
        }

        [Fact]
        public void TermsRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/home/terms")
                .To<HomeController>(c => c.Terms());
        }
    }
}
