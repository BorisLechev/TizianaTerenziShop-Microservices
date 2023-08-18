namespace TizianaTerenzi.Controllers.Tests.Controllers
{
    using MyTested.AspNetCore.Mvc;
    using TizianaTerenzi.Common.Enumerators;
    using TizianaTerenzi.WebClient.Controllers;
    using TizianaTerenzi.WebClient.ViewModels.Products;
    using Xunit;

    public class ProductsControllerTests
    {
        [Fact]
        public void AllShouldReturnCorrectViewWithModel()
        {
            MyController<ProductsController>
                .Instance()
                .Calling(c => c.All(null, ProductSorting.Default, 1))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ProductsListViewModel>());
        }
    }
}
