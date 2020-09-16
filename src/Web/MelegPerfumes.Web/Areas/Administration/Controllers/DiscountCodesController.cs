namespace MelegPerfumes.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Data;
    using MelegPerfumes.Web.Areas.Administration.InputModels;
    using MelegPerfumes.Web.Areas.Administration.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class DiscountCodesController : AdministrationController
    {
        private readonly IDiscountCodesService discountCodesService;

        public DiscountCodesController(IDiscountCodesService discountCodesService)
        {
            this.discountCodesService = discountCodesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiscountCodeViewModel>>> Index()
        {
            var discounts = (await this.discountCodesService
                .GetAllDiscountCodesAsync())
                .Select(dc => Mapper.Map<DiscountCodeViewModel>(dc))
                .ToList();

            return discounts;
        }

        [HttpPost]
        public async Task<IActionResult> Create(DiscountCodeInputModel discountCodeInputModel)
        {
            // TODO: check for valid inputModel - Cuber
            var discountName = discountCodeInputModel.Name.ToUpper();

            var discount = Mapper.Map<DiscountCode>(discountCodeInputModel);

            // TODO: check if this name already exist
            var result = await this.discountCodesService.CreateDiscountCodeAsync(discount);

            // TODO: Error Success - Cuber

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await this.discountCodesService.DeleteDiscountCodeAsync(id);

            return this.RedirectToAction("Index");
        }

        //[HttpPost]
        //[Route("/order/discount/{discountName}")]
        //public async Task<IActionResult> ApplyDiscountCode(string discountName)
        //{
        //    var discountCode = await this.discountCodesService.FindDiscountByNameAsync(discountName);
        //}
    }
}
