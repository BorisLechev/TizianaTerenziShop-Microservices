namespace TizianaTerenzi.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Services.Data.Discounts;

    [Authorize]
    [Route("[controller]/[action]")]
    public class DiscountCodesController : BaseController
    {
        private readonly IDiscountCodesService discountCodesService;

        public DiscountCodesController(IDiscountCodesService discountCodesService)
        {
            this.discountCodesService = discountCodesService;
        }

        [HttpPost("{discountName}")]
        public async Task<IActionResult> Apply(string discountName)
        {
            if (discountName == null || discountName.Length > 30)
            {
                this.Error(NotificationMessages.DiscountCodeError);

                return this.RedirectToAction(nameof(CartController.Index), "Cart");
            }

            var isExisting = await this.discountCodesService.CheckIfThereIsSuchaDiscountAsync(discountName);

            if (isExisting == false)
            {
                this.Error(NotificationMessages.DiscountCodeError);

                return this.RedirectToAction(nameof(CartController.Index), "Cart");
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await this.discountCodesService.ModifyThePricesAfterAppliedDiscountCodeAsync(discountName, userId);

            if (result == false)
            {
                this.Error(NotificationMessages.AlreadyAppliedDiscountCode);

                return this.RedirectToAction(nameof(CartController.Index), "Cart");
            }

            this.Success(NotificationMessages.SuccessfullyAppliedDiscountCode);

            return this.RedirectToAction(nameof(CartController.Index), "Cart");
        }

        [HttpPost("{discountName}")]
        public async Task<IActionResult> Delete(string discountName)
        {
            if (discountName == null || discountName.Length > 30)
            {
                this.Error(NotificationMessages.DiscountCodeError);

                return this.RedirectToAction("Index", "Cart");
            }

            var isExisting = await this.discountCodesService.CheckIfThereIsSuchaDiscountAsync(discountName);

            if (isExisting == false)
            {
                this.Error(NotificationMessages.DiscountCodeError);

                return this.RedirectToAction("Index", "Cart");
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await this.discountCodesService.ModifyThePricesAfterDeletedDiscountCodeAsync(userId);

            if (result == false)
            {
                this.Error(NotificationMessages.CannotDeleteDiscountCodeError);

                return this.RedirectToAction("Index", "Cart");
            }

            this.Success(NotificationMessages.SuccessfullyDeletedDiscountCode);

            return this.RedirectToAction("Index", "Cart");
        }
    }
}
