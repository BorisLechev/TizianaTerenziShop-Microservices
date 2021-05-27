namespace TizianaTerenzi.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Services.Data.Dashboard;
    using TizianaTerenzi.Services.Data.UsersInformation;
    using TizianaTerenzi.Web.ViewModels.Dashboard;

    public class UsersController : AdministrationController
    {
        private readonly IDashboardService dashboardService;

        private readonly IUsersInformationService usersService;

        public UsersController(
            IDashboardService dashboardService,
            IUsersInformationService usersService)
        {
            this.dashboardService = dashboardService;
            this.usersService = usersService;
        }

        public async Task<IActionResult> Roles()
        {
            var viewModel = await this.dashboardService.GetUsernamesRolesAsync();

            return this.View(viewModel);
        }

        public async Task<IActionResult> AddUserInRole(UsernamesRolesIndexViewModel viewModel)
        {
            string inputUsername = viewModel.AddUserInRole.Username;
            string inputRole = viewModel.AddUserInRole.Role;

            if (this.ModelState.IsValid)
            {
                var isUserAlreadyAddedInThisRole = await this.dashboardService.IsUserAlreadyAddedInRoleAsync(inputUsername, inputRole);

                if (isUserAlreadyAddedInThisRole)
                {
                    this.Error(NotificationMessages.UserIsAlreadyInThisRole);
                }
                else
                {
                    var result = await this.dashboardService.UpdateUserRoleAsync(inputUsername, inputRole);

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

            return this.RedirectToAction("Index", "Dashboard");
        }

        public async Task<IActionResult> AllUsers()
        {
            var viewModel = await this.usersService.GetAllUsersAsync();

            return this.View(viewModel);
        }
    }
}
