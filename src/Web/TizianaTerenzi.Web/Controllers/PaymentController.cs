namespace TizianaTerenzi.Web.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
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
        public async Task<IActionResult> Pay(ShippingDataInputModel inputModel)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.RedirectToAction(nameof(CartController.Checkout), "Cart");
            }

            var domain = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            var options = new SessionCreateOptions
            {
                CustomerEmail = inputModel.Email,
                SubmitType = "pay",
                BillingAddressCollection = "auto",
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    SetupFutureUsage = "off_session",
                },
                PaymentMethodTypes = new List<string>
                {
                  "card",
                  "sepa_debit",
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
                SuccessUrl = domain + "/payment/finish",
                CancelUrl = domain + "/cart/checkout",
            };

            var user = await this.userManager.GetUserAsync(this.User);

            await this.cartService.SaveShippingDataAsync(user, inputModel);

            var service = new SessionService();
            Session session = service.Create(options);

            return this.Json(new { id = session.Id });
        }

        [Route("payment/finish")]
        public async Task<IActionResult> Finish()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var isThereAnyProductsInTheUsersCart = await this.cartService.IsThereAnyProductsInTheUsersCartAsync(userId);

            if (isThereAnyProductsInTheUsersCart == false)
            {
                this.Error(NotificationMessages.EmptyCartError);

                return this.RedirectToAction(nameof(CartController.Index), "Cart");
            }

            var result = await this.cartService.CheckoutAsync(userId);

            if (result == false)
            {
                this.Error(NotificationMessages.ProcessOrderError);

                return this.RedirectToAction(nameof(CartController.Index), "Cart");
            }

            await this.cartService.DeleteAllProductsInTheCartByUserIdAsync(userId);

            return this.RedirectToAction(nameof(this.ThankYou));
        }

        [Route("payment/success")]
        public IActionResult ThankYou()
        {
            return this.View();
        }
    }
}
