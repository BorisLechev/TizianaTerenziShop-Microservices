namespace TizianaTerenzi.Web.Controllers
{
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Cart;
    using TizianaTerenzi.Services.Data.Countries;
    using TizianaTerenzi.Services.Data.Discounts;
    using TizianaTerenzi.Services.Data.Products;
    using TizianaTerenzi.Web.ViewModels.Orders;

    public class CartController : BaseController
    {
        private readonly ICartService cartService;

        private readonly IProductsService productsService;

        private readonly IDiscountCodesService discountCodesService;

        private readonly ICountriesService countriesService;

        private readonly UserManager<ApplicationUser> userManager;

        public CartController(
            ICartService cartService,
            IProductsService productsService,
            IDiscountCodesService discountCodesService,
            ICountriesService countriesService,
            UserManager<ApplicationUser> userManager)
        {
            this.cartService = cartService;
            this.productsService = productsService;
            this.discountCodesService = discountCodesService;
            this.countriesService = countriesService;
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

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [Authorize]
        [Route("/cart/discount/{discountName}/apply")]
        public async Task<IActionResult> ApplyDiscountCode(string discountName)
        {
            if (discountName == null || discountName.Length > 30)
            {
                this.Error(NotificationMessages.DiscountCodeError);

                return this.RedirectToAction(nameof(this.Index));
            }

            var isExisting = await this.discountCodesService.FindDiscountByNameAsync(discountName);

            if (isExisting == false)
            {
                this.Error(NotificationMessages.DiscountCodeError);

                return this.RedirectToAction(nameof(this.Index));
            }

            var userId = this.userManager.GetUserId(this.User);

            var result = await this.discountCodesService.ModifyThePricesAfterAppliedDiscountCodeAsync(discountName, userId);

            if (result == false)
            {
                this.Error(NotificationMessages.AlreadyAppliedDiscountCode);

                return this.RedirectToAction(nameof(this.Index));
            }

            this.Success(NotificationMessages.SuccessfullyAppliedDiscountCode);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [Authorize]
        [Route("/cart/discount/{discountName}/delete")]
        public async Task<IActionResult> DeleteDiscountCode(string discountName)
        {
            if (discountName == null || discountName.Length > 30)
            {
                this.Error(NotificationMessages.DiscountCodeError);

                return this.RedirectToAction(nameof(this.Index));
            }

            var isExisting = await this.discountCodesService.FindDiscountByNameAsync(discountName);

            if (isExisting == false)
            {
                this.Error(NotificationMessages.DiscountCodeError);

                return this.RedirectToAction(nameof(this.Index));
            }

            var userId = this.userManager.GetUserId(this.User);

            var result = await this.discountCodesService.ModifyThePricesAfterDeletedDiscountCodeAsync(userId);

            if (result == false)
            {
                this.Error(NotificationMessages.CannotDeleteDiscountCodeError);

                return this.RedirectToAction(nameof(this.Index));
            }

            this.Success(NotificationMessages.SuccessfullyDeletedDiscountCode);

            return this.RedirectToAction(nameof(this.Index));
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

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Checkout()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Login", "Authentication");
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var productsInTheCart = await this.cartService
                 .GetAllProductsInTheCartByUserId(user.Id);
            var countries = await this.countriesService.GetAllCountriesAsync();
            var bulgariaId = countries.Single(c => c.Text == "Bulgaria").Value;

            var viewModel = new OrderCheckoutViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                CountryId = user.CountryId,
                Town = user.Town,
                Address = user.Address,
                PostalCode = user.PostalCode,
                PhoneNumber = user.PhoneNumber,
                Products = productsInTheCart,
                Countries = countries,
                BulgariaId = int.Parse(bulgariaId),
            };

            if (!productsInTheCart.Any())
            {
                this.Error(NotificationMessages.EmptyCartError);

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveShippingData(OrderCheckoutViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null)
            {
                return this.NotFound();
            }

            await this.cartService.SaveShippingDataAsync(user, viewModel);

            var personalData = new
            {
                user.CountryId,
            };

            var json = JsonConvert.SerializeObject(personalData, Formatting.Indented);

            this.Response.Headers.Add("Content-Disposition",
                "attachment; filename=" + string.Format("{0}_PersonalData_{1}_{2}.json", GlobalConstants.SystemName, user.FirstName, user.LastName));

            return new FileContentResult(Encoding.UTF8.GetBytes(json), "text/json");
        }
    }
}
