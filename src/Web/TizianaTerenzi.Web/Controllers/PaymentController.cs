namespace TizianaTerenzi.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Cart;

    public class PaymentController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly ICartService cartService;

        public PaymentController(
            UserManager<ApplicationUser> userManager,
            ICartService cartService)
        {
            this.userManager = userManager;
            this.cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> Pay()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Login", "Authentication");
            }

            var userId = this.userManager.GetUserId(this.User);
            var productsInTheCart = await this.cartService.GetAllProductsInTheCartByUserId(userId);

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
