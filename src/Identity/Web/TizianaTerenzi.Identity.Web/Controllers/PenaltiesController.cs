namespace TizianaTerenzi.Identity.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Common.Web.ValidationAttributes;
    using TizianaTerenzi.Identity.Services.Data.UserPenalties;
    using TizianaTerenzi.Identity.Web.Models.UserPenalties;

    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PenaltiesController : ApiController
    {
        private readonly IUserPenaltiesService userPenaltiesService;

        public PenaltiesController(IUserPenaltiesService userPenaltiesService)
        {
            this.userPenaltiesService = userPenaltiesService;
        }

        [HttpGet]
        [AuthorizeAdministrator]
        public async Task<ActionResult<UserPenaltiesInputModel>> Index()
        {
            var viewModel = new UserPenaltiesInputModel
            {
                BlockedUsernames = await this.userPenaltiesService.GetAllBlockedUsersAsync(),
                UnblockedUsernames = await this.userPenaltiesService.GetAllUnblockedUsersAsync(),
            };

            return this.Ok(viewModel);
        }
    }
}
