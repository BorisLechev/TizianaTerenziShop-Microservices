namespace TizianaTerenzi.WebClient.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common.Enumerators;
    using TizianaTerenzi.WebClient.Services.Products;

    public class ProductsController : BaseController
    {
        private readonly IProductsService productsService;

        public ProductsController(
            IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public async Task<IActionResult> All(string search, ProductSorting sorting, int page = 1)
        {
            var productsViewModel = await this.productsService.All(search, sorting, page);

            return this.View(productsViewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                this.NotFound();
            }

            var productDetailsViewModel = await this.productsService.Details(id);

            if (!productDetailsViewModel.Succeeded)
            {
                return this.NotFound();
            }

            return this.View(productDetailsViewModel.Data);
        }
    }
}
