namespace TizianaTerenzi.WebClient.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Stripe.Checkout;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.WebClient.Infrastructure.Extensions;
    using TizianaTerenzi.WebClient.Services.Carts;
    using TizianaTerenzi.WebClient.ViewModels.Orders;

    [Authorize]
    [ApiController]
    public class PaymentController : BaseController
    {
        private readonly ICartsService cartsService;

        public PaymentController(
            ICartsService cartsService)
        {
            this.cartsService = cartsService;
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
                SuccessUrl = domain + "/payment/finish?sessionId={CHECKOUT_SESSION_ID}" +
                    $"&fullName={inputModel.FirstName} {inputModel.LastName}&email={inputModel.Email}&phone={inputModel.PhoneNumber}" +
                    $"&address={inputModel.Address}&country={inputModel.Country}&town={inputModel.Town}&postalCode={inputModel.PostalCode}",
                CancelUrl = domain + "/cart/checkout",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return this.Json(new { id = session.Id });
        }

        [Route("payment/finish")]
        public async Task<IActionResult> Finish([FromQuery] string sessionId, string fullName, string email, string phone, string address, string country, string town, string postalCode)
        {
            var userId = this.User.GetUserId();

            if (this.User.GetUserEmail() != email)
            {
                this.Error(NotificationMessages.SomethingWentWrong);

                return this.RedirectToAction(nameof(CartController.Index), "Cart");
            }

            var shippingData = new OrderGatewayModel
            {
                FullName = fullName,
                Email = email,
                PhoneNumber = phone,
                ShippingAddress = address,
                Country = country,
                Town = town,
                PostalCode = postalCode,
            };

            var result = await this.cartsService.Order(shippingData);

            if (!result)
            {
                this.Error(NotificationMessages.EmptyCartError);

                return this.RedirectToAction(nameof(CartController.Index), "Cart");
            }

            return this.RedirectToAction(nameof(this.ThankYou));
        }

        [Route("payment/success")]
        public IActionResult ThankYou()
        {
            return this.View();
        }
    }
}
