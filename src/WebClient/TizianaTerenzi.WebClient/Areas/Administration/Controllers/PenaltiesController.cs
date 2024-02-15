namespace TizianaTerenzi.WebClient.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.WebClient.Services.Administration;
    using TizianaTerenzi.WebClient.Services.Identity;
    using TizianaTerenzi.WebClient.ViewModels.UserPenalties;

    public class PenaltiesController : AdministrationController
    {
        private readonly IIdentityService identityService;
        private readonly IAdministrationService administrationService;

        public PenaltiesController(
            IIdentityService identityService,
            IAdministrationService administrationService)
        {
            this.identityService = identityService;
            this.administrationService = administrationService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await this.identityService.GetAllBlockedAndUnblockedUsersAsync();

            return this.View(viewModel);
        }

        public async Task<IActionResult> BlockUser(UserPenaltiesInputModel input)
        {
            if (this.ModelState.IsValid == false)
            {
                this.Error(NotificationMessages.BlockUserError);

                return this.RedirectToAction(nameof(this.Index));
            }

            var isBlocked = await this.administrationService.BlockUserAsync(input);

            if (isBlocked.Succeeded)
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

            var isUnblocked = await this.administrationService.UnblockUserAsync(input);

            if (isUnblocked.Succeeded)
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
