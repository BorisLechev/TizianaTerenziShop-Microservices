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
        public async Task<Result<PrepareDataForProductCreationAndProductEditingResponseModel>> PrepareDropdownsForProductCreation()
        {
            var notes = await this.productsService.GetAllNotesAsync();
            var productTypes = await this.productsService.GetAllProductTypesAsync();
            var fragranceGroups = await this.productsService.GetAllFragranceGroupsAsync();

            var response = new PrepareDataForProductCreationAndProductEditingResponseModel
            {
                ProductTypes = productTypes,
                FragranceGroups = fragranceGroups,
                Notes = notes,
            };

            return Result<PrepareDataForProductCreationAndProductEditingResponseModel>.SuccessWith(response);
        }

        // TODO: Use gRPC
        [Authorize]
        [HttpGet]
        public async Task<Result<PrepareDataForProductCreationAndProductEditingResponseModel>> PrepareDropdownsForProductEditing(int productId)
        {
            var notes = await this.productsService.GetAllNotesWithSelectedByProductIdAsync(productId);
            var productTypes = await this.productsService.GetAllProductTypesAsync();
            var fragranceGroups = await this.productsService.GetAllFragranceGroupsAsync();

            var response = new PrepareDataForProductCreationAndProductEditingResponseModel
            {
                ProductTypes = productTypes,
                FragranceGroups = fragranceGroups,
                Notes = notes,
            };

            return Result<PrepareDataForProductCreationAndProductEditingResponseModel>.SuccessWith(response);
        }

        // TODO: Use gRPC
        [Authorize]
        [HttpGet]
        public async Task<Result<EditProductWithDropdownsResponseModel>> PrepareDataForProductEditing(int productId)
        {
            var editProductInputModel = await this.productsService.GetProductForEditingAsync(productId);
            var notes = await this.productsService.GetAllNotesWithSelectedByProductIdAsync(productId);
            var productTypes = await this.productsService.GetAllProductTypesAsync();
            var fragranceGroups = await this.productsService.GetAllFragranceGroupsAsync();

            var response = new EditProductWithDropdownsResponseModel
            {
                Name = editProductInputModel.Name,
                Description = editProductInputModel.Description,
                FragranceGroupId = editProductInputModel.FragranceGroupId,
                ProductTypeId = editProductInputModel.ProductTypeId,
                NoteIds = editProductInputModel.NoteIds,
                Picture = editProductInputModel.Picture,
                Price = editProductInputModel.Price,
                YearOfManufacture = editProductInputModel.YearOfManufacture,
                ProductTypes = productTypes,
                FragranceGroups = fragranceGroups,
                Notes = notes,
            };

            return Result<EditProductWithDropdownsResponseModel>.SuccessWith(response);
        }
    }
}
