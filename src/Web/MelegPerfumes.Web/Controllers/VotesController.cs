namespace MelegPerfumes.Web.Controllers
{
    using System.Threading.Tasks;

    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Data;
    using MelegPerfumes.Web.ViewModels.Votes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

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

            await this.votesService.VoteAsync(inputModel.CommentId, userId, inputModel.IsUpVote);

            var votesCount = this.votesService.GetVotes(inputModel.CommentId);

            return new VoteResponseModel { VotesCount = votesCount };
        }
    }
}
