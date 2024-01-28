namespace TizianaTerenzi.Products.Web.Gateway.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Products.Web.Gateway.Models;
    using TizianaTerenzi.Products.Web.Gateway.Services.Products;

    public class ProductsController : ApiController
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        // TODO: Use gRPC
        [Authorize]
        [HttpGet]
        public async Task<Result<PrepareDataForProductCreationResponseModel>> PrepareDropdownsForProductCreation()
        {
            var notes = await this.productsService.GetAllNotesAsync();
            var productTypes = await this.productsService.GetAllProductTypesAsync();
            var fragranceGroups = await this.productsService.GetAllGetAllFragranceGroupsAsyncNotesAsync();

            var response = new PrepareDataForProductCreationResponseModel
            {
                ProductTypes = productTypes,
                FragranceGroups = fragranceGroups,
                Notes = notes,
            };

            return Result<PrepareDataForProductCreationResponseModel>.SuccessWith(response);
        }
    }
}
