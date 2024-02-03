namespace TizianaTerenzi.Administration.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Administration.Services.Data.Products;
    using TizianaTerenzi.Administration.Web.Models.Products;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Common.Web.ValidationAttributes;

    [AuthorizeAdministrator]
    public class ProductsController : ApiController
    {
        private readonly IProductsService productsService;

        public ProductsController(
            IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [HttpPost]
        public async Task<ActionResult<Result>> Create([FromQuery] CreateProductInputModel inputModel, IFormFile picture)
        {
            byte[] pictureAsByteArray;

            using (var memoryStream = new MemoryStream())
            {
                await picture.CopyToAsync(memoryStream);

                pictureAsByteArray = memoryStream.ToArray();
            }

            await this.productsService.CreateProductAsync(inputModel, pictureAsByteArray);

            return Result.Success(NotificationMessages.CreateProductSuccessfully);
        }

        [HttpPut]
        public async Task<ActionResult<Result>> Edit([FromQuery] EditProductInputModel inputModel, IFormFile picture)
        {
            byte[]? pictureAsByteArray = null;

            if (picture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await picture.CopyToAsync(memoryStream);

                    pictureAsByteArray = memoryStream.ToArray();
                }

                pictureAsByteArray = pictureAsByteArray.Length == 0 ? null : pictureAsByteArray;
            }

            await this.productsService.EditProductAsync(inputModel, pictureAsByteArray);

            return Result.Success(NotificationMessages.EditProductSuccessfully);
        }

        [HttpDelete]
        public async Task<ActionResult<Result>> Delete(int productId)
        {
            await this.productsService.DeleteProductAsync(productId);

            return Result.Success(NotificationMessages.DeleteProductSuccessfully);
        }
    }
}
