namespace TizianaTerenzi.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Services.Data;

    public class ProductsController : BaseController
    {
        private const int ItemsPerPage = 6;

        private readonly IProductsService productsService;

        public ProductsController(
            IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public async Task<IActionResult> All(int page = 1)
        {
            page = Math.Max(1, page);
            var skip = (page - 1) * ItemsPerPage;
            var products = await this.productsService.GetAllProductsAsync(ItemsPerPage, skip);

            products.ItemsCount = await this.productsService.GetProductsCountAsync();
            products.ItemsPerPage = ItemsPerPage;

            products.CurrentPage = page;

            return this.View(products);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id <= 0)
            {
                this.NotFound();
            }

            var productDetailsViewModel = await this.productsService.GetProductDetailsByIdAsync(id);

            if (productDetailsViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(productDetailsViewModel);
        }
    }
}
