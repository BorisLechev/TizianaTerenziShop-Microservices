namespace TizianaTerenzi.Administration.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Administration.Services.Data.Users;
    using TizianaTerenzi.Administration.Web.Models.Users;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Common.Web.ValidationAttributes;

    [AuthorizeAdministrator]
    public class UsersController : ApiController
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpPost]
        public async Task<ActionResult<Result>> AddUserInRole(UsernamesRolesIndexViewModel viewModel)
        {
            await this.usersService.AddUserInRole(viewModel);

            return this.Ok();
        }
    }
}
