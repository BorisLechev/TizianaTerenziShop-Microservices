namespace TizianaTerenzi.Carts.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Carts.Services.Data.Discounts;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;

    [Authorize]
    public class DiscountCodesController : ApiController
    {
        private readonly IDiscountCodesService discountCodesService;

        public DiscountCodesController(IDiscountCodesService discountCodesService)
        {
            this.discountCodesService = discountCodesService;
        }

        [HttpPost]
        public async Task<ActionResult<Result>> Apply(string discountName)
        {
            if (string.IsNullOrWhiteSpace(discountName) || discountName.Length > 30)
            {
                return Result.Failure(NotificationMessages.DiscountCodeError);
            }

            var isExisting = await this.discountCodesService.CheckIfThereIsSuchaDiscountAsync(discountName);

            if (!isExisting)
            {
                return Result.Failure(NotificationMessages.DiscountCodeError);
            }

            var userId = this.User.GetUserId();

            var result = await this.discountCodesService.ModifyThePricesAfterAppliedDiscountCodeAsync(discountName, userId);

            if (!result)
            {
                return Result.Failure(NotificationMessages.AlreadyAppliedDiscountCode);
            }

            return Result.Success(NotificationMessages.SuccessfullyAppliedDiscountCode);
        }

        [HttpDelete]
        public async Task<ActionResult<Result>> Delete(string discountName)
        {
            if (string.IsNullOrWhiteSpace(discountName) || discountName.Length > 30)
            {
                return Result.Failure(NotificationMessages.DiscountCodeError);
            }

            var isExisting = await this.discountCodesService.CheckIfThereIsSuchaDiscountAsync(discountName);

            if (!isExisting)
            {
                return Result.Failure(NotificationMessages.DiscountCodeError);
            }

            var userId = this.User.GetUserId();

            var result = await this.discountCodesService.ModifyThePricesAfterDeletedDiscountCodeAsync(discountName, userId);

            if (!result)
            {
                return Result.Failure(NotificationMessages.CannotDeleteDiscountCodeError);
            }

            return Result.Success(NotificationMessages.SuccessfullyDeletedDiscountCode);
        }
    }
}
