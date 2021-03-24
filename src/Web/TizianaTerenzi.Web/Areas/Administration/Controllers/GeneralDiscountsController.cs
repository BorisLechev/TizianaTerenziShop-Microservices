namespace TizianaTerenzi.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Services.Data.Discounts;
    using TizianaTerenzi.Services.Data.Products;
    using TizianaTerenzi.Web.ViewModels.GeneralDiscounts;

    [ApiController]
    [Route("administration/[controller]/[action]")]
    public class GeneralDiscountsController : AdministrationController
    {
        private readonly IGeneralDiscountsService generalDiscountsService;

        private readonly IProductsService productsService;

        public GeneralDiscountsController(
            IGeneralDiscountsService generalDiscountsService,
            IProductsService productsService)
        {
            this.generalDiscountsService = generalDiscountsService;
            this.productsService = productsService;
        }

        public async Task<IActionResult> Index()
        {
            var range = Enumerable.Range(0, 100)
                .Select(n => new SelectListItem
                {
                    Value = n.ToString(),
                    Text = n.ToString(),
                })
                .ToList();

            var generalDiscount = await this.generalDiscountsService.GetGeneralDiscountAsync<GeneralDiscountViewModel>();
            generalDiscount.Percents = range;

            return this.View(generalDiscount);
        }

        [HttpPost]
        public async Task<IActionResult> Apply(GeneralDiscountInputModel inputModel)
        {
            var productsCount = await this.productsService.UpdateThePricesOfAllProductsAfterTheDiscountIsAppliedAsync(inputModel.Percent);

            if (productsCount == 0)
            {
                this.Error(NotificationMessages.CannotApplyOrDisableGeneralDiscount);

                return this.LocalRedirect("/products/all");
            }

            var generalDiscountsResult = await this.generalDiscountsService.ApplyDiscountToAllProductsAsync(inputModel.Percent);

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
            var productsCount = await this.productsService.UpdateThePricesOfAllProductsAfterTheDiscountIsDisabledAsync();

            if (productsCount == 0)
            {
                this.Error(NotificationMessages.CannotApplyOrDisableGeneralDiscount);

                return this.LocalRedirect("/products/all");
            }

            var resultAfterDisable = await this.generalDiscountsService.DisableDiscountToAllProductsAsync();

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
