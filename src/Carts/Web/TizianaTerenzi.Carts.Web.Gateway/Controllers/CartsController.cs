namespace TizianaTerenzi.Carts.Web.Gateway.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Carts.Web.Gateway.Services.Carts;
    using TizianaTerenzi.Carts.Web.Gateway.Services.Identity;
    using TizianaTerenzi.Carts.Web.Models.Carts;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;

    public class CartsController : ApiController
    {
        private readonly IIdentityService identityService;
        private readonly ICartsService cartsService;

        public CartsController(
            IIdentityService identityService,
            ICartsService cartsService)
        {
            this.identityService = identityService;
            this.cartsService = cartsService;
        }

        // TODO: Use gRPC
        [Authorize]
        [HttpGet]
        public async Task<Result<ProductsInTheCartCheckoutViewModel>> Checkout()
        {
            var userId = this.User.GetUserId();
            var user = await this.identityService.GetProfileData(userId);

            if (!user.Succeeded)
            {
                return Result<ProductsInTheCartCheckoutViewModel>.Failure(NotificationMessages.InvalidPassword);
            }

            var productsInTheCart = await this.cartsService.GetAllProductsInTheCart();

            var countries = await this.cartsService.GetAllCountries();
            var bulgariaId = countries.Single(c => c.Text == "Bulgaria").Value;

            var result = new ProductsInTheCartCheckoutViewModel
            {
                Email = user.Data.Email,
                FirstName = user.Data.FirstName,
                LastName = user.Data.LastName,
                CountryId = user.Data.Country != null ? int.Parse(countries.Single(c => c.Text == user.Data.Country).Value) : null,
                Town = user.Data.Town,
                Address = user.Data.Address,
                PostalCode = user.Data.PostalCode,
                PhoneNumber = user.Data.Phone,
                Products = productsInTheCart,
                Countries = countries,
                BulgariaId = int.Parse(bulgariaId),
                DiscountCodeId = productsInTheCart.Select(p => p.DiscountCodeId).FirstOrDefault(),
                DiscountCodeDiscount = productsInTheCart.Select(p => p.DiscountCodeDiscount).FirstOrDefault(),
            };

            return Result<ProductsInTheCartCheckoutViewModel>.SuccessWith(result);
        }
    }
}
