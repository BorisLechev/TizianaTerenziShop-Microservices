namespace MelegPerfumes.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using MelegPerfumes.Services.Data;
    using MelegPerfumes.Services.Models;
    using MelegPerfumes.Web.ViewModels.Products;
    using Microsoft.AspNetCore.Mvc;

    public class ProductsController : BaseController
    {
        private readonly IProductsService productsService;

        private readonly ICartService cartService;

        public ProductsController(
            IProductsService productsService,
            ICartService cartService)
        {
            this.productsService = productsService;
            this.cartService = cartService;
        }

        public async Task<IActionResult> All()
        {
            var products = (await this.productsService.GetAllProductsAsync()).ToList();

            return this.View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await this.productsService.GetProductByIdAsync(id);

            if (product == null) // TODO: add other rules
            {
                return this.NotFound();
            }

            return this.View(product);
        }

        //[HttpPost]
        public async Task<IActionResult> Order(int productId)
        {
            var product = await this.productsService.GetProductByIdAsync(productId);

            var productServiceModel = new ProductInTheCartServiceModel
            {
                Id = Guid.NewGuid().ToString(),
                IssuerId = this.User.FindFirstValue(ClaimTypes.NameIdentifier),
                ProductId = product.Id,
                Quantity = 1,
            };

            await this.cartService.AddProductInTheCart(productServiceModel);

            return this.Redirect("/Order/Cart");
        }
    }
}
