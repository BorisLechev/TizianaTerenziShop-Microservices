namespace TizianaTerenzi.Administration.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Administration.Services.Data.UserPenalties;
    using TizianaTerenzi.Administration.Web.Models.UserPenalties;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Common.Web.ValidationAttributes;

    [AuthorizeAdministrator]
    public class PenaltiesController : ApiController
    {
        private readonly IUserPenaltiesService userPenaltiesService;

        public PenaltiesController(IUserPenaltiesService userPenaltiesService)
        {
            this.userPenaltiesService = userPenaltiesService;
        }

        [HttpPost]
        public async Task<ActionResult<Result>> BlockUser(UserPenaltiesInputModel inputModel)
        {
            await this.userPenaltiesService.BlockUserAsync(inputModel.UserId, inputModel.ReasonToBeBlocked);

            return Result.Success(NotificationMessages.SuccessfullyBlockedUser);
        }

        [HttpPut]
        public async Task<ActionResult<Result>> UnblockUser(UserPenaltiesInputModel inputModel)
        {
            await this.userPenaltiesService.UnblockUserAsync(inputModel.UserId);

            return Result.Success(NotificationMessages.SuccessfullyUnblockedUser);
        }
    }
}
