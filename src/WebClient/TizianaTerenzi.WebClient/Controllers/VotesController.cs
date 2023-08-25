namespace TizianaTerenzi.WebClient.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.WebClient.Services.Products;
    using TizianaTerenzi.WebClient.ViewModels.Votes;

    [Authorize]
    [ApiController]
    public class VotesController : BaseController
    {
        private readonly IProductsService productsService;

        public VotesController(
            IProductsService productsService)
        {
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

            return this.Ok(result.Data);
        }

        [HttpPost]
        [Route("votes/product/post")]
        public async Task<ActionResult<ProductVoteResponseModel>> Vote(PostProductVoteInputModel inputModel)
        {
            var result = await this.productsService.VoteForProduct(inputModel);

            if (!result.Succeeded)
            {
                return this.BadRequest();
            }

            return this.Ok(result.Data);
        }
    }
}
