namespace TizianaTerenzi.Administration.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Administration.Services.Data.DiscountCodes;
    using TizianaTerenzi.Administration.Web.Models.DiscountCodes;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Common.Web.ValidationAttributes;

    [AuthorizeAdministratorAttribute]
    public class DiscountCodesController : ApiController
    {
        private readonly IDiscountCodesService discountCodesService;

        public DiscountCodesController(IDiscountCodesService discountCodesService)
        {
            this.discountCodesService = discountCodesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiscountCodesListingViewModel>>> Index()
        {
            var discountCodes = await this.discountCodesService.GetAllDiscountCodesAsync<DiscountCodesListingViewModel>();

            return this.Ok(discountCodes);
        }

        [HttpPost]
        public async Task<ActionResult<Result>> Create(CreateDiscountCodeInputModel inputModel)
        {
            var result = await this.discountCodesService.CreateDiscountCodeAsync(inputModel);

            if (!result)
            {
                return Result.Failure(NotificationMessages.CreateDiscountCodeError);
            }

            return Result.Success(NotificationMessages.CreateDiscountCodeSuccessfully);
        }

        [HttpDelete]
        public async Task<ActionResult<Result>> Delete(int discountCodeId)
        {
            var result = await this.discountCodesService.DeleteDiscountCodeAsync(discountCodeId);

            if (!result)
            {
                return Result.Failure(NotificationMessages.CannotDeleteDiscountCodeError);
            }

            return Result.Success(NotificationMessages.SuccessfullyDeletedDiscountCode);
        }
    }
}
