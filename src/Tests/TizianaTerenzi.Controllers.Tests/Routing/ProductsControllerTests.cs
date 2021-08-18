namespace TizianaTerenzi.Controllers.Tests.Routing
{
    using MyTested.AspNetCore.Mvc;
    using TizianaTerenzi.Web.Controllers;
    using Xunit;

    public class ProductsControllerTests
    {
        [Fact]
        public void DetailsShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/products/details/1")
                .To<ProductsController>(c => c.Details(1));
        }
    }
}
