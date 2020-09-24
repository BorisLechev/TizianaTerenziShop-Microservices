namespace MelegPerfumes.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Data;
    using MelegPerfumes.Services.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ProductsController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IProductsService productsService;

        private readonly ICartService cartService;

        public ProductsController(
            UserManager<ApplicationUser> userManager,
            IProductsService productsService,
            ICartService cartService)
        {
            this.userManager = userManager;
            this.productsService = productsService;
            this.cartService = cartService;
        }

        public async Task<IActionResult> All()
        {
            var products = (await this.productsService
                .GetAllProductsAsync())
                .ToList();

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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Order(int productId)
        {
            var userId = this.userManager.GetUserId(this.User);
            var product = await this.productsService.GetProductByIdAsync(productId);
            var productsInTheCart = await this.cartService.GetAllProductsInTheCartByUserId(userId);

            // TODO: refactoring
            if (productsInTheCart.Any(p => p.ProductId == productId))
            {
                var productInTheCart = await this.cartService.GetProductById(productId);

                await this.cartService.IncreaseQuantity(productInTheCart.Id);
            }
            else
            {
                var productInTheCart = new ProductInTheCart
                {
                    Id = Guid.NewGuid().ToString(),
                    IssuerId = userId,
                    ProductId = product.Id,
                    Quantity = 1,
                };

                await this.cartService.AddProductInTheCart(productInTheCart);
            }

            return this.Redirect("/Order/Cart");
        }
    }
}
