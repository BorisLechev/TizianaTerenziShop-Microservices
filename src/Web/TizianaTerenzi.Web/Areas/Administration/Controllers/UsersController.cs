namespace TizianaTerenzi.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Services.Data.UserRoles;
    using TizianaTerenzi.Web.ViewModels.UserRoles;

    public class UsersController : AdministrationController
    {
        private readonly IUserRolesService userRolesService;

        public UsersController(IUserRolesService userRolesService)
        {
            this.userRolesService = userRolesService;
        }

        public async Task<IActionResult> Roles()
        {
            var viewModel = await this.userRolesService.GetUsernamesRolesAsync();

            return this.View(viewModel);
        }

        public async Task<IActionResult> AddUserInRole(UsernamesRolesIndexViewModel viewModel)
        {
            string inputUsername = viewModel.AddUserInRole.Username;
            string inputRole = viewModel.AddUserInRole.Role;

            if (this.ModelState.IsValid)
            {
                var isUserAlreadyAddedInThisRole = await this.userRolesService.IsUserAlreadyAddedInRoleAsync(inputUsername, inputRole);

                if (isUserAlreadyAddedInThisRole)
                {
                    this.Error(NotificationMessages.UserIsAlreadyInThisRole);
                }
                else
                {
                    var result = await this.userRolesService.UpdateUserRoleAsync(inputUsername, inputRole);

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
            var viewModel = await this.userRolesService.GetAllUsersAsync();

            return this.View(viewModel);
        }
    }
}
