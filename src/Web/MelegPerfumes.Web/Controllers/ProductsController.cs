namespace MelegPerfumes.Web.Controllers
{
    using System.Threading.Tasks;

    using MelegPerfumes.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    public class ProductsController : BaseController
    {
        private readonly IProductsService productsService;

        public ProductsController(
            IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public async Task<IActionResult> All()
        {
            var products = await this.productsService.GetAllProductsAsync();

            return this.View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var productDetailsViewModel = await this.productsService.GetProductByIdAsync(id);

            if (productDetailsViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(productDetailsViewModel);
        }
    }
}
