namespace TizianaTerenzi.WebClient.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.WebClient.Services.Administration;
    using TizianaTerenzi.WebClient.ViewModels.GeneralDiscounts;

    [ApiController]
    [Route("[area]/[controller]/[action]")]
    public class GeneralDiscountsController : AdministrationController
    {
        private readonly IAdministrationService administrationService;

        public GeneralDiscountsController(IAdministrationService administrationService)
        {
            this.administrationService = administrationService;
        }

        public async Task<IActionResult> Index()
        {
            var generalDiscount = await this.administrationService.GetGeneralDiscountsAsync();

            return this.View(generalDiscount);
        }

        [HttpPost]
        public async Task<IActionResult> Apply(GeneralDiscountInputModel inputModel)
        {
            var generalDiscountsResult = await this.administrationService.ApplyGeneralDiscountToAllProductsAsync(inputModel);

            if (generalDiscountsResult == false)
            {
                this.Error(NotificationMessages.CannotApplyOrDisableGeneralDiscount);

                return this.LocalRedirect("/products/all");
            }

            this.Success(NotificationMessages.SuccessfullyAppliedGeneralDiscount);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> Disable()
        {
            var resultAfterDisable = await this.administrationService.DisableGeneralDiscountToAllProductsAsync();

            if (resultAfterDisable == false)
            {
                this.Error(NotificationMessages.CannotApplyOrDisableGeneralDiscount);

                return this.LocalRedirect("/products/all");
            }

            this.Success(NotificationMessages.SuccessfullyDisabledGeneralDiscount);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
