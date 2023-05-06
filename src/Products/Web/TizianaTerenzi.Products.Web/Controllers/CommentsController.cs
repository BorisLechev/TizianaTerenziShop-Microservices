namespace TizianaTerenzi.Products.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Services.Identity;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Products.Services.Data.Comments;
    using TizianaTerenzi.Products.Web.Models.Comments;

    [Authorize]
    public class CommentsController : ApiController
    {
        private readonly ICommentsService commentsService;
        private readonly ICurrentUserService currentUserService;

        public CommentsController(
            ICommentsService commentsService,
            ICurrentUserService currentUserService)
        {
            this.commentsService = commentsService;
            this.currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<ActionResult<Result>> Create(CreateCommentInputModel inputModel)
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

            var userId = this.currentUserService.UserId;

            var result = await this.commentsService.CreateAsync(inputModel, userId);

            if (result == false)
            {
                return Result.Failure(NotificationMessages.CreateCommentError);
            }

            return Result.Success(NotificationMessages.CreateCommentSuccessfully);
        }
    }
}
