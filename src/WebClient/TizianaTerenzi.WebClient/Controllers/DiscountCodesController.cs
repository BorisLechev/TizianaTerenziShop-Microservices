namespace TizianaTerenzi.WebClient.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.WebClient.Services.Carts;

    [Authorize]
    [Route("[controller]/[action]")]
    public class DiscountCodesController : BaseController
    {
        private readonly ICartsService cartsService;

        public DiscountCodesController(ICartsService cartsService)
        {
            this.cartsService = cartsService;
        }

        [HttpPost]
        public async Task<IActionResult> Apply(string discountName)
        {
            var result = await this.cartsService.ApplyDiscountCode(discountName);

            if (!result.Succeeded)
            {
                this.Error(result.Errors.FirstOrDefault());

                return this.RedirectToAction(nameof(CartController.Index), "Cart");
            }

            this.Success(result.SucceessMessage);

            return this.RedirectToAction(nameof(CartController.Index), "Cart");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string discountName)
        {
            var result = await this.cartsService.DeleteDiscountCode(discountName);

            if (!result.Succeeded)
            {
                this.Error(result.Errors.FirstOrDefault());

                return this.RedirectToAction(nameof(CartController.Index), "Cart");
            }

            this.Success(result.SucceessMessage);

            return this.RedirectToAction(nameof(CartController.Index), "Cart");
        }
    }
}
