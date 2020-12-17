namespace TizianaTerenzi.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Cart;
    using TizianaTerenzi.Services.Data.DiscountCodes;
    using TizianaTerenzi.Services.Data.Products;

    public class CartController : BaseController
    {
        private readonly ICartService cartService;

        private readonly IProductsService productsService;

        private readonly IDiscountCodesService discountCodesService;

        private readonly UserManager<ApplicationUser> userManager;

        public CartController(
            ICartService cartService,
            IProductsService productsService,
            IDiscountCodesService discountCodesService,
            UserManager<ApplicationUser> userManager)
        {
            this.cartService = cartService;
            this.productsService = productsService;
            this.discountCodesService = discountCodesService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = this.userManager.GetUserId(this.User);

            var productsInTheCart = await this.cartService.GetAllProductsInTheCartByUserId(userId);

            return this.View(productsInTheCart);
        }

        [HttpPost]
        [Route("/cart/{productId}/quantity/increase")]
        public async Task<IActionResult> IncreaseQuantity(string productId)
        {
            bool result = await this.cartService.IncreaseQuantity(productId);

            if (result)
            {
                return this.Ok();
            }
            else
            {
                return this.Forbid();
            }
        }

        [HttpPost]
        [Route("/cart/{productId}/quantity/reduce")]
        public async Task<IActionResult> ReduceQuantity(string productId)
        {
            bool result = await this.cartService.ReduceQuantity(productId);

            if (result)
            {
                return this.Ok();
            }
            else
            {
                return this.Forbid();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddProductInTheCart(int productId)
        {
            if (productId <= 0)
            {
                return this.NotFound();
            }

            var userId = this.userManager.GetUserId(this.User);
            var product = await this.productsService.GetProductByIdAsync(productId);

            if (product == null)
            {
                return this.NotFound();
            }

            var ifProductInTheCartExists = await this.cartService.CheckIfProductByUserIdExistInTheCartAsync(userId, product.Id);

            if (ifProductInTheCartExists == true)
            {
                var productInTheCartId = await this.cartService.GetProductInTheCartIdByProductIdAsync(productId);

                await this.cartService.IncreaseQuantity(productInTheCartId);
            }
            else
            {
                await this.cartService.AddProductInTheCart(product, userId);
            }

            return this.RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        [Authorize]
        [Route("/cart/discount/{discountName}/apply")]
        public async Task<IActionResult> ApplyDiscountCode(string discountName)
        {
            if (discountName == null || discountName.Length > 30)
            {
                this.Error(NotificationMessages.DiscountCodeError);

                return this.RedirectToAction("Index", "Cart");
            }

            var discountCode = await this.discountCodesService.GetDiscountByNameAsync(discountName);

            if (discountCode == null)
            {
                this.Error(NotificationMessages.DiscountCodeError);

                return this.RedirectToAction("Index", "Cart");
            }

            var userId = this.userManager.GetUserId(this.User);

            var result = await this.discountCodesService.ModifyThePricesAfterAppliedDiscountCodeAsync(discountCode, userId);

            if (result == false)
            {
                this.Error(NotificationMessages.AlreadyAppliedDiscountCode);

                return this.RedirectToAction("Index", "Cart");
            }

            this.Success(NotificationMessages.SuccessfullyAppliedDiscountCode);

            return this.RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        [Authorize]
        [Route("/cart/discount/{discountName}/delete")]
        public async Task<IActionResult> DeleteDiscountCode(string discountName)
        {
            if (discountName == null || discountName.Length > 30)
            {
                this.Error(NotificationMessages.DiscountCodeError);

                return this.RedirectToAction("Index", "Cart");
            }

            var discountCode = await this.discountCodesService.GetDiscountByNameAsync(discountName);

            if (discountCode == null)
            {
                this.Error(NotificationMessages.DiscountCodeError);

                return this.RedirectToAction("Index", "Cart");
            }

            var userId = this.userManager.GetUserId(this.User);

            var result = await this.discountCodesService.ModifyThePricesAfterDeletedDiscountCodeAsync(userId);

            if (result == false)
            {
                this.Error(NotificationMessages.CannotDeleteDiscountCodeError);

                return this.RedirectToAction("Index", "Cart");
            }

            this.Success(NotificationMessages.SuccessfullyDeletedDiscountCode);

            return this.RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            bool result = await this.cartService.DeleteProductInTheCart(id);

            if (!result)
            {
                this.Error(NotificationMessages.CannotDeleteThisProductInTheCartError);
            }

            return this.RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Login", "Authentication");
            }

            var userId = this.userManager.GetUserId(this.User);
            var productsInTheCart = await this.cartService
                 .GetAllProductsInTheCartByUserId(userId);

            if (!productsInTheCart.Any())
            {
                this.Error(NotificationMessages.EmptyCartError);

                return this.RedirectToAction("Index", "Cart");
            }

            await this.cartService.CheckOutAsync(userId, productsInTheCart);

            await this.cartService.DeleteAllProductsInTheCartByUserId(userId);

            return this.RedirectToAction("Index", "Home");
        }
    }
}
