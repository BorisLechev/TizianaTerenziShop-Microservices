namespace TizianaTerenzi.Products.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Products.Services.Data.Comments;
    using TizianaTerenzi.Products.Web.Models.Comments;

    [Authorize]
    public class CommentsController : ApiController
    {
        private readonly ICommentsService commentsService;

        public CommentsController(
            ICommentsService commentsService)
        {
            this.commentsService = commentsService;
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
                    return this.BadRequest(Result.Failure(NotificationMessages.CreateCommentError));
                }
            }

            var userId = this.User.GetUserId();

            var result = await this.commentsService.CreateAsync(inputModel, userId);

            if (result == false)
            {
                return this.BadRequest(Result.Failure(NotificationMessages.CreateCommentError));
            }

            return this.Ok(Result.Success(NotificationMessages.CreateCommentSuccessfully));
        }

        public async Task<ActionResult<IEnumerable<UsersCommentsPersonalDataResponseModel>>> GetAllUsersCommentsAndVotesPersonalData()
        {
            var userId = this.User.GetUserId();

            var usersCommentsAndVotes = await this.commentsService.GetAllUsersCommentsAndVotesPersonalData(userId);

            return this.Ok(usersCommentsAndVotes);
        }

        [HttpDelete]
        public async Task<ActionResult<Result>> DeleteAllUserComments()
        {
            var userId = this.User.GetUserId();

            var result = await this.commentsService.DeleteRangeByUserIdAsync(userId);

            if (!result)
            {
                return Result.Failure(NotificationMessages.SomethingWentWrong);
            }

            return this.Ok(Result.Success());
        }
    }
}
