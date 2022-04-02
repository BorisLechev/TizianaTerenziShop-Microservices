namespace TizianaTerenzi.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Services.Data.Comments;
    using TizianaTerenzi.Web.Infrastructure.Extensions;
    using TizianaTerenzi.Web.ViewModels.Comments;

    [Authorize]
    public class CommentsController : BaseController
    {
        private readonly ICommentsService commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentInputModel inputModel)
        {
            var parentId = inputModel.ParentId == 0 ? null : inputModel.ParentId;
            inputModel.ParentId = parentId;

            // security
            if (parentId.HasValue)
            {
                var isInProductId = await this.commentsService.IsInProductIdAsync(parentId.Value, inputModel.ProductId);

                if (isInProductId == false)
                {
                    return this.Forbid();
                }
            }

            var userId = this.User.GetUserId();

            var result = await this.commentsService.CreateAsync(inputModel, userId);

            if (result == false)
            {
                this.Error(NotificationMessages.CreateCommentError);

                return this.RedirectToAction(nameof(ProductsController.Details), "Products", new { id = inputModel.ProductId });
            }

            this.Success(NotificationMessages.CreateCommentSuccessfully);

            return this.RedirectToAction(nameof(ProductsController.Details), "Products", new { id = inputModel.ProductId });
        }
    }
}
