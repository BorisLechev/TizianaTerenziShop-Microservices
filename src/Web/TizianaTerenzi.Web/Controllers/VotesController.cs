namespace TizianaTerenzi.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
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

        public VotesController(
            ICommentVotesService commentVotesService,
            IProductVotesService productVotesService)
        {
            this.commentVotesService = commentVotesService;
            this.productVotesService = productVotesService;
        }

        /// POST /votes/comment/post
        /// Request body: {"commentId":1,"isUpVote":true}
        /// Response body: {"votesCount":16}
        [HttpPost]
        [Route("votes/comment/post")]
        public async Task<ActionResult<CommentVoteResponseModel>> Vote(PostCommentVoteInputModel inputModel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

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
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await this.productVotesService.VoteAsync(inputModel.ProductId, userId, inputModel.Value);

            if (result == false)
            {
                return this.BadRequest();
            }

            var groupedProductVotes = await this.productVotesService.GetNumberOfVotesForEachValueAsync(inputModel.ProductId);
            var numberOfVoters = groupedProductVotes.CountOfVotes;
            var averageVotes = groupedProductVotes.AverageVotes;

            var countOfVotesWithValueFive = groupedProductVotes.GroupVotesWithValue5;
            var countOfVotesWithValueFour = groupedProductVotes.GroupVotesWithValue4;
            var countOfVotesWithValueThree = groupedProductVotes.GroupVotesWithValue3;
            var countOfVotesWithValueTwo = groupedProductVotes.GroupVotesWithValue2;
            var countOfVotesWithValueOne = groupedProductVotes.GroupVotesWithValue1;

            var responseModel = new ProductVoteResponseModel
            {
                AverageVote = averageVotes,
                NumberOfVoters = numberOfVoters,
                ProductId = inputModel.ProductId,
                ShareOfVotesWithValueOfFive = countOfVotesWithValueFive > 0
                                                ? (double)countOfVotesWithValueFive / numberOfVoters * 100
                                                : 0,
                ShareOfVotesWithValueOfFour = countOfVotesWithValueFour > 0
                                                ? (double)countOfVotesWithValueFour / numberOfVoters * 100
                                                : 0,
                ShareOfVotesWithValueOfThree = countOfVotesWithValueThree > 0
                                                ? (double)countOfVotesWithValueThree / numberOfVoters * 100
                                                : 0,
                ShareOfVotesWithValueOfTwo = countOfVotesWithValueTwo > 0
                                                ? (double)countOfVotesWithValueTwo / numberOfVoters * 100
                                                : 0,
                ShareOfVotesWithValueOfOne = countOfVotesWithValueOne > 0
                                                ? (double)countOfVotesWithValueOne / numberOfVoters * 100
                                                : 0,
            };

            return responseModel;
        }
    }
}
