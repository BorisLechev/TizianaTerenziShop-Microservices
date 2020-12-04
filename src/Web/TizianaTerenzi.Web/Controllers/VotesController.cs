namespace TizianaTerenzi.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data;
    using TizianaTerenzi.Web.ViewModels.Votes;

    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : BaseController
    {
        private readonly IVotesService votesService;

        private readonly UserManager<ApplicationUser> userManager;

        public VotesController(
            IVotesService votesService,
            UserManager<ApplicationUser> userManager)
        {
            this.votesService = votesService;
            this.userManager = userManager;
        }

        /// POST /api/votes
        /// Request body: {"commentId":1,"isUpVote":true}
        /// Response body: {"votesCount":16}
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<VoteResponseModel>> Vote(VotesInputModel inputModel)
        {
            var userId = this.userManager.GetUserId(this.User);

            var result = await this.votesService.VoteAsync(inputModel.CommentId, userId);

            if (result == false)
            {
                return this.BadRequest();
            }

            var votesCount = await this.votesService.GetVotesAsync(inputModel.CommentId);
            var vote = await this.votesService.GetVoteAsync(inputModel.CommentId, userId);
            var isUpVoted = userId == vote.UserId && vote.Type == VoteType.UpVote;

            return new VoteResponseModel { VotesCount = votesCount, IsUpVoted = isUpVoted };
        }
    }
}
