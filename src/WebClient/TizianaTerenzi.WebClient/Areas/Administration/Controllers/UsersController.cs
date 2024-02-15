namespace TizianaTerenzi.WebClient.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.WebClient.Services.Administration;
    using TizianaTerenzi.WebClient.Services.Identity;
    using TizianaTerenzi.WebClient.ViewModels.Users;

    public class UsersController : AdministrationController
    {
        private readonly IIdentityService identityService;
        private readonly IAdministrationService administrationService;

        public UsersController(
            IIdentityService identityService,
            IAdministrationService administrationService)
        {
            this.identityService = identityService;
            this.administrationService = administrationService;
        }

        public async Task<IActionResult> Roles()
        {
            var viewModel = await this.identityService.GetUsernamesRolesAsync();

            return this.View(viewModel);
        }

        public async Task<IActionResult> AddUserInRole(UsernamesRolesIndexViewModel viewModel)
        {
            string inputUserId = viewModel.UserId;
            string inputRoleId = viewModel.RoleId;

            if (this.ModelState.IsValid)
            {
                //var isUserAlreadyAddedInThisRole = await this.usersService.IsUserAlreadyAddedInRoleAsync(inputUserId, inputRoleId);

                //if (isUserAlreadyAddedInThisRole)
                //{
                //    this.Error(NotificationMessages.UserIsAlreadyInThisRole);
                //}
                //else
                //{
                //    var result = await this.usersService.UpdateUserRoleAsync(inputUserId, inputRoleId);

                //    if (result)
                //    {
                //        this.Success(NotificationMessages.SuccessfullyAddedUserInRole);
                //    }
                //}
                var result = await this.administrationService.AddUserInRole(viewModel);

                if (result)
                {
                    this.Success(NotificationMessages.SuccessfullyAddedUserInRole);
                }
            }
            else
            {
                return this.View(viewModel);
            }

            return this.RedirectToAction(nameof(this.Roles));
        }

        public async Task<IActionResult> AllUsers()
        {
            var viewModel = await this.identityService.GetAllUsersAsync();

            return this.View(viewModel);
        }

        public async Task<IActionResult> AllBannedUsers()
        {
            var viewModel = await this.identityService.GetAllBannedUsersAsync();

            return this.View(viewModel);
        }
    }
}
