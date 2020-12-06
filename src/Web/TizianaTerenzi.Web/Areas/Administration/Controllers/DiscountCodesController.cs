namespace TizianaTerenzi.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data;
    using TizianaTerenzi.Web.Areas.Administration.Models.DiscountCodes;

    public class DiscountCodesController : AdministrationController
    {
        private readonly IDiscountCodesService discountCodesService;

        public DiscountCodesController(IDiscountCodesService discountCodesService)
        {
            this.discountCodesService = discountCodesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiscountCodesListingViewModel>>> Index()
        {
            var discountCodes = await this.discountCodesService.GetAllDiscountCodesAsync();
            var discounts = discountCodes
                .Select(dc => new DiscountCodesListingViewModel
                {
                    Id = dc.Id,
                    Name = dc.Name,
                    CreatedOn = dc.CreatedOn,
                    Discount = dc.Discount,
                    ExpiresOn = dc.ExpiresOn,
                })
                .ToList();

            return this.View(discounts);
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

            var discount = new DiscountCode
            {
                Name = discountCodeInputModel.Name,
                Discount = discountCodeInputModel.Discount,
                ExpiresOn = discountCodeInputModel.ExpiresOn,
            };

            var result = await this.discountCodesService.CreateDiscountCodeAsync(discount);

            if (result == false)
            {
                this.Error(NotificationMessages.CreateDiscountCodeError);

                return this.RedirectToAction("Index", "Home");
            }

            this.Success(NotificationMessages.CreateDiscountCodeSuccessfully);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int discountCodeId)
        {
            var result = await this.discountCodesService.DeleteDiscountCodeAsync(discountCodeId);

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
