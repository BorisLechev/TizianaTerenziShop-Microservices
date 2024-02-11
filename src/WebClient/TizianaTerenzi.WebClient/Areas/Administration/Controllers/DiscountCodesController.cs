namespace TizianaTerenzi.WebClient.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.WebClient.Services.Administration;
    using TizianaTerenzi.WebClient.ViewModels.DiscountCodes;

    public class DiscountCodesController : AdministrationController
    {
        private readonly IAdministrationService administrationService;

        public DiscountCodesController(IAdministrationService administrationService)
        {
            this.administrationService = administrationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiscountCodesListingViewModel>>> Index()
        {
            var discountCodes = await this.administrationService.GetAllDiscountCodesAsync();

            return this.View(discountCodes);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDiscountCodeInputModel discountCodeInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(discountCodeInputModel);
            }

            discountCodeInputModel.Name.ToUpper();

            var result = await this.administrationService.CreateDiscountCodeAsync(discountCodeInputModel);

            if (result == false)
            {
                this.Error(NotificationMessages.CreateDiscountCodeError);

                return this.RedirectToAction(nameof(this.Index));
            }

            this.Success(NotificationMessages.CreateDiscountCodeSuccessfully);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int discountCodeId)
        {
            var result = await this.administrationService.DeleteDiscountCodeAsync(discountCodeId);

            if (result == false)
            {
                this.Error(NotificationMessages.CannotDeleteDiscountCodeError);

                return this.RedirectToAction(nameof(this.Index));
            }

            this.Success(NotificationMessages.SuccessfullyDeletedDiscountCode);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
