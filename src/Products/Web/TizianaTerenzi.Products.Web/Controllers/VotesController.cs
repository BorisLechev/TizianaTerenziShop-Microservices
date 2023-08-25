namespace TizianaTerenzi.Products.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Products.Data.Models;
    using TizianaTerenzi.Products.Services.Data.Votes;
    using TizianaTerenzi.Products.Web.Models.Votes;

    [Authorize]
    public class VotesController : ApiController
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
        [Route("/votes/voteForComment")]
        public async Task<Result<CommentVoteResponseModel>> Vote(PostCommentVoteInputModel inputModel)
        {
            var userId = this.User.GetUserId();

            var result = await this.commentVotesService.VoteAsync(inputModel.CommentId, userId);

            if (!result)
            {
                return Result<CommentVoteResponseModel>.Failure("Bad request");
            }

            var votesCount = await this.commentVotesService.GetVotesAsync(inputModel.CommentId);
            var vote = await this.commentVotesService.GetVoteAsync(inputModel.CommentId, userId);
            var isUpVoted = userId == vote.UserId && vote.Type == CommentVoteType.UpVote;

            return Result<CommentVoteResponseModel>.SuccessWith(new CommentVoteResponseModel { VotesCount = votesCount, IsUpVoted = isUpVoted });
        }

        [HttpPost]
        [Route("/votes/voteForProduct")]
        public async Task<Result<ProductVoteResponseModel>> Vote(PostProductVoteInputModel inputModel)
        {
            var userId = this.User.GetUserId();

            var result = await this.productVotesService.VoteAsync(inputModel.ProductId, userId, inputModel.Value);

            if (!result)
            {
                return Result<ProductVoteResponseModel>.Failure("Bad request");
            }

            var groupedProductVotes = await this.productVotesService.GetNumberOfVotesForEachValueAsync(inputModel.ProductId);
            var numberOfVoters = groupedProductVotes != null
                                    ? groupedProductVotes.CountOfVotes
                                    : 0;
            var averageVotes = groupedProductVotes != null
                                ? groupedProductVotes.AverageVotes
                                : 0;

            var countOfVotesWithValueFive = groupedProductVotes?.GroupVotesWithValue5;
            var countOfVotesWithValueFour = groupedProductVotes?.GroupVotesWithValue4;
            var countOfVotesWithValueThree = groupedProductVotes?.GroupVotesWithValue3;
            var countOfVotesWithValueTwo = groupedProductVotes?.GroupVotesWithValue2;
            var countOfVotesWithValueOne = groupedProductVotes?.GroupVotesWithValue1;

            var responseModel = new ProductVoteResponseModel
            {
                AverageVote = averageVotes,
                NumberOfVoters = numberOfVoters,
                ProductId = inputModel.ProductId,
                ShareOfVotesWithValueOfFive = countOfVotesWithValueFive.HasValue && numberOfVoters > 0
                                                ? (double)countOfVotesWithValueFive / numberOfVoters * 100
                                                : 0,
                ShareOfVotesWithValueOfFour = countOfVotesWithValueFour.HasValue && numberOfVoters > 0
                                                ? (double)countOfVotesWithValueFour / numberOfVoters * 100
                                                : 0,
                ShareOfVotesWithValueOfThree = countOfVotesWithValueThree.HasValue && numberOfVoters > 0
                                                ? (double)countOfVotesWithValueThree / numberOfVoters * 100
                                                : 0,
                ShareOfVotesWithValueOfTwo = countOfVotesWithValueTwo.HasValue && numberOfVoters > 0
                                                ? (double)countOfVotesWithValueTwo / numberOfVoters * 100
                                                : 0,
                ShareOfVotesWithValueOfOne = countOfVotesWithValueOne.HasValue && numberOfVoters > 0
                                                ? (double)countOfVotesWithValueOne / numberOfVoters * 100
                                                : 0,
            };

            return Result<ProductVoteResponseModel>.SuccessWith(responseModel);
        }
    }
}
