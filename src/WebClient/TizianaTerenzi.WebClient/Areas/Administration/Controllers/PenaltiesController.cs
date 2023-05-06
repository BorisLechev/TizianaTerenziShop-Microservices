namespace TizianaTerenzi.WebClient.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Services.Data.UserPenalties;
    using TizianaTerenzi.Web.ViewModels.UserPenalties;

    public class PenaltiesController : AdministrationController
    {
        private readonly IUserPenaltiesService userPenaltiesService;

        public PenaltiesController(IUserPenaltiesService userPenaltiesService)
        {
            this.userPenaltiesService = userPenaltiesService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new UserPenaltiesInputModel
            {
                BlockedUsernames = await this.userPenaltiesService.GetAllBlockedUsersAsync(),
                UnblockedUsernames = await this.userPenaltiesService.GetAllUnblockedUsersAsync(),
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> BlockUser(UserPenaltiesInputModel input)
        {
            if (this.ModelState.IsValid == false)
            {
                this.Error(NotificationMessages.BlockUserError);

                return this.RedirectToAction(nameof(this.Index));
            }

            var isBlocked = await this.userPenaltiesService.BlockUserAsync(input.UserId, input.ReasonToBeBlocked);

            if (isBlocked)
            {
                this.Success(NotificationMessages.SuccessfullyBlockedUser);
            }
            else
            {
                this.Error(NotificationMessages.UserIsAlreadyBlocked);
            }

            return this.RedirectToAction(nameof(UsersController.AllBannedUsers), "Users");
        }

        public async Task<IActionResult> UnblockUser(UserPenaltiesInputModel input)
        {
            if (this.ModelState.IsValid == false)
            {
                this.Error(NotificationMessages.UnblockUserError);

                return this.RedirectToAction(nameof(this.Index));
            }

            var isUnblocked = await this.userPenaltiesService.UnblockUserAsync(input.UserId);

            if (isUnblocked)
            {
                this.Success(NotificationMessages.SuccessfullyUnblockedUser);
            }
            else
            {
                this.Error(NotificationMessages.UserIsAlreadyUnblocked);
            }

            return this.RedirectToAction(nameof(UsersController.AllUsers), "Users");
        }
    }
}
