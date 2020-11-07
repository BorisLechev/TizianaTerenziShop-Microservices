namespace MelegPerfumes.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using MelegPerfumes.Common;
    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

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
        [Route("/cart/index")]
        public async Task<IActionResult> Index()
        {
            var userId = this.userManager.GetUserId(this.User);

            var productsInTheCart = await this.cartService.GetAllProductsInTheCartByUserId(userId);

            return this.View(productsInTheCart);
        }

        [HttpGet]
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

        [HttpGet]
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
            var userId = this.userManager.GetUserId(this.User);
            var product = await this.productsService.GetProductByIdAsync(productId);

            if (product == null)
            {
                return this.NotFound();
            }

            var ifProductInTheCartExists = await this.cartService.CheckIfProductByUserIdExistInTheCartAsync(userId, product.Id);

            if (ifProductInTheCartExists == true)
            {
                var productInTheCart = await this.cartService.GetProductById(productId);

                await this.cartService.IncreaseQuantity(productInTheCart.Id);
            }
            else
            {
                var productInTheCart = new ProductInTheCart
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    ProductId = product.Id,
                    Quantity = 1,
                    ProductPriceAfterDiscount = product.Price,
                };

                await this.cartService.AddProductInTheCart(productInTheCart);
            }

            return this.RedirectToAction("Index", "Cart");
        }

        // TODO: make it post
        //[HttpPost]
        [Authorize]
        [Route("/cart/discount/{discountName}/apply")]
        public async Task<IActionResult> ApplyDiscountCode(string discountName)
        {
            // TODO: user input model
            //if (!this.ModelState.IsValid)
            //{
            //    return this.View(inputModel);
            //}

            var discountCode = await this.discountCodesService.GetDiscountByNameAsync(discountName);

            if (discountCode == null)
            {
                this.Error(NotificationMessages.DiscountCodeError);

                return this.Forbid(); // TODO: change it
            }

            var userId = this.userManager.GetUserId(this.User);

            var result = await this.discountCodesService.ModifyThePricesAfterAppliedDiscountCodeAsync(discountCode, userId);

            if (result == false)
            {
                this.Error(NotificationMessages.AlreadyAppliedDiscountCode);

                return this.RedirectToAction("Index", "Cart");
            }

            //this.Success(NotificationMessages.SuccessfullyAppliedDiscountCode);

            return this.RedirectToAction("Index", "Cart");
        }

        // TODO: make it post
        //[HttpPost]
        [Authorize]
        [Route("/cart/discount/{discountName}/delete")]
        public async Task<IActionResult> DeleteDiscountCode(string discountName)
        {
            // TODO: user input model
            //if (!this.ModelState.IsValid)
            //{
            //    return this.View(inputModel);
            //}

            var discountCode = await this.discountCodesService.GetDiscountByNameAsync(discountName);

            if (discountCode == null)
            {
                this.Error(NotificationMessages.DiscountCodeError);

                return this.Forbid(); // TODO: change it
            }

            var userId = this.userManager.GetUserId(this.User);

            var result = await this.discountCodesService.ModifyThePricesAfterDeletedDiscountCodeAsync(discountCode, userId);

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

        // TODO: make it Httppost
        //[HttpPost]
        [Route("/cart/checkout")]
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

            var orderProducts = productsInTheCart
                .Select(op => new OrderProduct
                {
                    ProductId = op.ProductId,
                    Price = op.ProductPriceAfterDiscount,
                    Quantity = op.Quantity,
                    CreatedOn = DateTime.UtcNow,
                    UserId = userId,
                    DiscountCodeId = op.DiscountCodeId,
                })
                .ToList();

            var discountCodeId = productsInTheCart.FirstOrDefault().DiscountCodeId;

            await this.cartService.CheckOutAsync(userId, orderProducts, discountCodeId);

            await this.cartService.DeleteAllProductsInTheCartByUserId(userId);

            return this.RedirectToAction("Index", "Home");
        }
    }
}
