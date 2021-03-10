namespace TizianaTerenzi.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Stripe.Checkout;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Cart;
    using TizianaTerenzi.Web.ViewModels.Orders;

    [Authorize]
    [ApiController]
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
        [Route("payment/pay")]
        public async Task<IActionResult> Pay(ShippingDataInputModel viewModel)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Login", "Authentication");
            }

            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Checkout", "Cart");
            }

            var domain = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            var options = new SessionCreateOptions
            {
                CustomerEmail = "customer@example.com",
                SubmitType = "pay",
                BillingAddressCollection = "auto",
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    SetupFutureUsage = "off_session",
                },
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                      UnitAmountDecimal = 2000,
                      Currency = "eur",
                      ProductData = new SessionLineItemPriceDataProductDataOptions
                      {
                        Name = "Product",
                      },
                    },
                    Quantity = 1,
                  },
                },
                Mode = "payment",
                SuccessUrl = domain + "/home/index",
                CancelUrl = domain + "/cart/checkout",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            var user = await this.userManager.GetUserAsync(this.User);

            await this.cartService.SaveShippingDataAsync(user, viewModel);

            var productsInTheCart = await this.cartService.GetAllProductsInTheCartByUserId(user.Id);

            if (!productsInTheCart.Any())
            {
                this.Error(NotificationMessages.EmptyCartError);

                return this.RedirectToAction("Index", "Cart");
            }

            var result = await this.cartService.CheckOutAsync(user.Id, productsInTheCart);

            if (result == false)
            {
                this.Error(NotificationMessages.ProcessOrderError);

                return this.RedirectToAction("Index", "Cart");
            }

            await this.cartService.DeleteAllProductsInTheCartByUserId(user.Id);

            return this.Json(new { id = session.Id });
        }
    }
}
