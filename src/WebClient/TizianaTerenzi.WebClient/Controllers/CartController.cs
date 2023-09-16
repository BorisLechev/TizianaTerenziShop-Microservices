namespace TizianaTerenzi.WebClient.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Cart;
    using TizianaTerenzi.Services.Data.Countries;
    using TizianaTerenzi.WebClient.Services.Carts;
    using TizianaTerenzi.WebClient.Services.Products;
    using TizianaTerenzi.WebClient.ViewModels.Orders;

    [Authorize]
    public class CartController : BaseController
    {
        private readonly ICartService cartService;

        private readonly ICountriesService countriesService;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly IProductsService productsService;

        private readonly ICartsService cartsService;

        public CartController(
            ICartService cartService,
            ICountriesService countriesService,
            UserManager<ApplicationUser> userManager,
            IProductsService productsService,
            ICartsService cartsService)
        {
            this.cartService = cartService;
            this.countriesService = countriesService;
            this.userManager = userManager;
            this.productsService = productsService;
            this.cartsService = cartsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var productsInTheCart = await this.cartsService.GetAllProductsInTheUsersCart();

            return this.View(productsInTheCart);
        }

        [HttpPost]
        public async Task<IActionResult> IncreaseQuantity(string productId)
        {
            var result = await this.cartsService.IncreaseQuantity(productId);

            if (result.Succeeded)
            {
                return this.Ok();
            }
            else
            {
                return this.Forbid();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReduceQuantity(string productId)
        {
            var result = await this.cartsService.ReduceQuantity(productId);

            if (result.Succeeded)
            {
                return this.Ok();
            }
            else
            {
                return this.Forbid();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProductInTheCart(int productId)
        {
            await this.productsService.AddProductInTheCart(productId);

            return this.NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var result = await this.cartsService.DeleteProductInTheCart(id);

            if (!result.Succeeded)
            {
                this.Error(NotificationMessages.CannotDeleteThisProductInTheCartError);
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Checkout()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var productsInTheCart = await this.cartService
                 .GetAllProductsInTheCartByUserIdAsync(user.Id);
            var countries = await this.countriesService.GetAllCountriesAsync();
            var bulgariaId = countries.Single(c => c.Text == "Bulgaria").Value;

            var viewModel = new OrderCheckoutViewModel
            {
                Email = user.Email,
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

            if (productsInTheCart.Any() == false)
            {
                this.Error(NotificationMessages.EmptyCartError);

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View("~/Views/Payment/Index.cshtml", viewModel);
        }
    }
}
