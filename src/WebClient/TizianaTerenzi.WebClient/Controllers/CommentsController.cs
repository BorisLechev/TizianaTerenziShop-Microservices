namespace TizianaTerenzi.WebClient.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.WebClient.Services.Products;
    using TizianaTerenzi.WebClient.ViewModels.Comments;

    [Authorize]
    public class CommentsController : BaseController
    {
        private readonly IProductsService productsService;

        public CommentsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentInputModel inputModel)
        {
            var result = await this.productsService.CreateProductComment(inputModel);

            if (!result)
            {
                this.Error(NotificationMessages.CreateCommentError);

                return this.RedirectToAction(nameof(ProductsController.Details), "Products", new { id = inputModel.ProductId });
            }

            this.Success(NotificationMessages.CreateCommentSuccessfully);

            return this.RedirectToAction(nameof(ProductsController.Details), "Products", new { id = inputModel.ProductId });
        }
    }
}
