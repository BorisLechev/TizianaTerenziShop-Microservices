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

            var productsCount = await this.productsService.GetProductsCountAsync();

            products.PagesCount = (int)Math.Ceiling(productsCount / (decimal)ItemsPerPage);

            if (products.PagesCount == 0)
            {
                products.PagesCount = 1;
            }

            products.CurrentPage = page;

            return this.View(products);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
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
