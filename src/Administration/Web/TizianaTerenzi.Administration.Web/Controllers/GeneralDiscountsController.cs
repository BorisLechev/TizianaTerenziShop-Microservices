namespace TizianaTerenzi.Administration.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using TizianaTerenzi.Administration.Services.Data.GeneralDiscounts;
    using TizianaTerenzi.Administration.Web.Models.GeneralDiscounts;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Common.Web.ValidationAttributes;

    [AuthorizeAdministratorAttribute]
    public class GeneralDiscountsController : ApiController
    {
        private readonly IGeneralDiscountsService generalDiscountsService;

        public GeneralDiscountsController(IGeneralDiscountsService generalDiscountsService)
        {
            this.generalDiscountsService = generalDiscountsService;
        }

        [HttpGet]
        public async Task<ActionResult<GeneralDiscountViewModel>> Index()
        {
            var range = Enumerable.Range(0, 101)
                .Select(n => new SelectListItem
                {
                    Value = n.ToString(),
                    Text = n.ToString(),
                })
                .ToList();

            var generalDiscount = await this.generalDiscountsService.GetGeneralDiscountAsync<GeneralDiscountViewModel>();
            generalDiscount.Percents = range;

            return generalDiscount;
        }

        [HttpPost]
        public async Task<ActionResult<Result>> Apply(GeneralDiscountInputModel inputModel)
        {
            var generalDiscountsResult = await this.generalDiscountsService.ApplyDiscountToAllProductsAsync(inputModel.Percent);

            if (generalDiscountsResult == false)
            {
                return Result.Failure(NotificationMessages.CannotApplyOrDisableGeneralDiscount);
            }

            return Result.Success(NotificationMessages.SuccessfullyAppliedGeneralDiscount);
        }

        [HttpPost]
        public async Task<ActionResult<Result>> Disable()
        {
            var resultAfterDisable = await this.generalDiscountsService.DisableDiscountToAllProductsAsync();

            if (resultAfterDisable == false)
            {
                return Result.Failure(NotificationMessages.CannotApplyOrDisableGeneralDiscount);
            }

            return Result.Success(NotificationMessages.SuccessfullyDisabledGeneralDiscount);
        }
    }
}
