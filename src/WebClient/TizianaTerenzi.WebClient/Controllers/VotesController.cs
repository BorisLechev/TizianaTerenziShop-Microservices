namespace TizianaTerenzi.WebClient.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Services.Data.Votes;
    using TizianaTerenzi.WebClient.Infrastructure.Extensions;
    using TizianaTerenzi.WebClient.Services.Products;
    using TizianaTerenzi.WebClient.ViewModels.Votes;

    [Authorize]
    [ApiController]
    public class VotesController : BaseController
    {
        private readonly IProductVotesService productVotesService;

        private readonly IProductsService productsService;

        public VotesController(
            IProductVotesService productVotesService,
            IProductsService productsService)
        {
            this.productVotesService = productVotesService;
            this.productsService = productsService;
        }

        /// POST /votes/comment/post
        /// Request body: {"commentId":1,"isUpVote":true}
        /// Response body: {"votesCount":16}
        [HttpPost]
        [Route("votes/comment/post")]
        public async Task<ActionResult<CommentVoteResponseModel>> Vote(PostCommentVoteInputModel inputModel)
        {
            var result = await this.productsService.VoteForComment(inputModel);

            if (!result.Succeeded)
            {
                return this.BadRequest();
            }

            return result.Data;
        }

        [HttpPost]
        [Route("votes/product/post")]
        public async Task<ActionResult<ProductVoteResponseModel>> Vote(PostProductVoteInputModel inputModel)
        {
            var userId = this.User.GetUserId();

            var result = await this.productVotesService.VoteAsync(inputModel.ProductId, userId, inputModel.Value);

            if (result == false)
            {
                return this.BadRequest();
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

            return responseModel;
        }
    }
}
