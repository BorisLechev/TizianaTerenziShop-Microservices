namespace TizianaTerenzi.WebClient.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Services.Data.Users;
    using TizianaTerenzi.Web.ViewModels.Users;

    public class UsersController : AdministrationController
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public async Task<IActionResult> Roles()
        {
            var viewModel = await this.usersService.GetUsernamesRolesAsync();

            return this.View(viewModel);
        }

        public async Task<IActionResult> AddUserInRole(UsernamesRolesIndexViewModel viewModel)
        {
            string inputUserId = viewModel.UserId;
            string inputRoleId = viewModel.RoleId;

            if (this.ModelState.IsValid)
            {
                var isUserAlreadyAddedInThisRole = await this.usersService.IsUserAlreadyAddedInRoleAsync(inputUserId, inputRoleId);

                if (isUserAlreadyAddedInThisRole)
                {
                    this.Error(NotificationMessages.UserIsAlreadyInThisRole);
                }
                else
                {
                    var result = await this.usersService.UpdateUserRoleAsync(inputUserId, inputRoleId);

                    if (result)
                    {
                        this.Success(NotificationMessages.SuccessfullyAddedUserInRole);
                    }
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
            var viewModel = await this.usersService.GetAllUsersAsync();

            return this.View(viewModel);
        }

        public async Task<IActionResult> AllBannedUsers()
        {
            var viewModel = await this.usersService.GetAllBannedUsersAsync();

            return this.View(viewModel);
        }
    }
}
