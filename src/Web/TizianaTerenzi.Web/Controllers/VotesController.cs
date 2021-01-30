namespace TizianaTerenzi.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Votes;
    using TizianaTerenzi.Web.ViewModels.Votes;

    [Authorize]
    [ApiController]
    public class VotesController : BaseController
    {
        private readonly ICommentVotesService commentVotesService;

        private readonly IProductVotesService productVotesService;

        private readonly UserManager<ApplicationUser> userManager;

        public VotesController(
            ICommentVotesService commentVotesService,
            IProductVotesService productVotesService,
            UserManager<ApplicationUser> userManager)
        {
            this.commentVotesService = commentVotesService;
            this.productVotesService = productVotesService;
            this.userManager = userManager;
        }

        /// POST /votes/comment/post
        /// Request body: {"commentId":1,"isUpVote":true}
        /// Response body: {"votesCount":16}
        [HttpPost]
        [Route("votes/comment/post")]
        public async Task<ActionResult<CommentVoteResponseModel>> Vote(PostCommentVoteInputModel inputModel)
        {
            var userId = this.userManager.GetUserId(this.User);

            var result = await this.commentVotesService.VoteAsync(inputModel.CommentId, userId);

            if (result == false)
            {
                return this.BadRequest();
            }

            var votesCount = await this.commentVotesService.GetVotesAsync(inputModel.CommentId);
            var vote = await this.commentVotesService.GetVoteAsync(inputModel.CommentId, userId);
            var isUpVoted = userId == vote.UserId && vote.Type == CommentVoteType.UpVote;

            return new CommentVoteResponseModel { VotesCount = votesCount, IsUpVoted = isUpVoted };
        }

        [HttpPost]
        [Route("votes/product/post")]
        public async Task<ActionResult<ProductVoteResponseModel>> Vote(PostProductVoteInputModel inputModel)
        {
            var userId = this.userManager.GetUserId(this.User);

            var result = await this.productVotesService.VoteAsync(inputModel.ProductId, userId, inputModel.Value);

            if (result == false)
            {
                return this.BadRequest();
            }

            var averageVotes = await this.productVotesService.GetAverageVotesAsync(inputModel.ProductId);
            var numberOfVoters = await this.productVotesService.GetNumberOfVotersAsync(inputModel.ProductId);

            return new ProductVoteResponseModel { AverageVote = averageVotes, NumberOfVoters = numberOfVoters, ProductId = inputModel.ProductId };
        }
    }
}
