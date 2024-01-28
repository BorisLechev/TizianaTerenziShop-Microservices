namespace TizianaTerenzi.Products.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Products.Services.Data.ProductTypes;

    public class ProductTypesController : ApiController
    {
        private readonly IProductTypesService productTypesService;

        public ProductTypesController(IProductTypesService productTypesService)
        {
            this.productTypesService = productTypesService;
        }

        [HttpGet]
        public async Task<IEnumerable<SelectListItem>> Index()
        {
            var notes = await this.productTypesService.GetAllProductTypesAsync();

            return notes;
        }
    }
}
