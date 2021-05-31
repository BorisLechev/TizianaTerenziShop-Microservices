namespace TizianaTerenzi.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Comments;
    using TizianaTerenzi.Web.ViewModels.Comments;

    [Authorize]
    public class CommentsController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly ICommentsService commentsService;

        public CommentsController(
            UserManager<ApplicationUser> userManager,
            ICommentsService commentsService)
        {
            this.userManager = userManager;
            this.commentsService = commentsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentInputModel inputModel)
        {
            var parentId = inputModel.ParentId == 0 ? (int?)null : inputModel.ParentId;
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

            var userId = this.userManager.GetUserId(this.User);

            var result = await this.commentsService.CreateAsync(inputModel, userId);

            if (result == false)
            {
                this.Error(NotificationMessages.CreateCommentError);

                return this.RedirectToAction("Details", "Products", new { id = inputModel.ProductId });
            }

            this.Success(NotificationMessages.CreateCommentSuccessfully);

            return this.RedirectToAction("Details", "Products", new { id = inputModel.ProductId });
        }
    }
}
