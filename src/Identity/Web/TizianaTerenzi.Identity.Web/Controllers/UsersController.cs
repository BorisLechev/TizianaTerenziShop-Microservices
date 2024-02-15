namespace TizianaTerenzi.Identity.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Common.Web.ValidationAttributes;
    using TizianaTerenzi.Identity.Services.Data.Users;
    using TizianaTerenzi.Identity.Web.Models.Users;

    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UsersController : ApiController
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet]
        public async Task<ActionResult<UsernamesRolesIndexViewModel>> Roles()
        {
            var viewModel = await this.usersService.GetUsernamesRolesAsync();

            return this.Ok(viewModel);
        }

        [HttpGet]
        [AuthorizeAdministrator]
        public async Task<ActionResult<IEnumerable<ApplicationUserViewModel>>> AllUsers()
        {
            var allUsers = await this.usersService.GetAllUsersAsync();

            return this.Ok(allUsers);
        }

        [HttpGet]
        [AuthorizeAdministrator]
        public async Task<ActionResult<IEnumerable<BannedApplicationUserViewModel>>> AllBannedUsers()
        {
            var bannedUsers = await this.usersService.GetAllBannedUsersAsync();

            return this.Ok(bannedUsers);
        }
    }
}
