namespace TizianaTerenzi.Services.Data.UserRoles
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Web.ViewModels.UserRoles;

    public class UserRolesService : IUserRolesService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly RoleManager<ApplicationRole> roleManager;

        public UserRolesService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this.usersRepository = usersRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<AllUsersViewModel> GetAllUsersAsync()
        {
            var allUsers = await this.usersRepository
                .AllAsNoTracking()
                .ToListAsync();

            var allUsersViewModel = new AllUsersViewModel();

            foreach (var user in allUsers)
            {
                var userRoles = await this.userManager.GetRolesAsync(user);

                var viewModel = new ApplicationUserViewModel
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = userRoles[0],
                    CreatedOn = user.CreatedOn,
                    Town = user.Town,
                    Address = user.Address,
                };

                allUsersViewModel.ApplicationUsers.Add(viewModel);
            }

            return allUsersViewModel;
        }

        public async Task<UsernamesRolesIndexViewModel> GetUsernamesRolesAsync()
        {
            var usernames = await this.usersRepository
                .All()
                .Select(u => u.UserName)
                .ToListAsync();

            var viewModel = new UsernamesRolesIndexViewModel
            {
                Usernames = usernames,
            };

            return viewModel;
        }

        public async Task<bool> IsUserAlreadyAddedInRoleAsync(string inputUsername, string inputRole)
        {
            var user = await this.userManager.FindByNameAsync(inputUsername);
            IdentityRole newRole = await this.roleManager.FindByNameAsync(inputRole);

            if (user == null || newRole == null)
            {
                return false;
            }

            var isUserAlreadyAddedInRole = await this.userManager.IsInRoleAsync(user, newRole.Name);

            if (isUserAlreadyAddedInRole)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateUserRoleAsync(string username, string inputRole)
        {
            var user = await this.userManager.FindByNameAsync(username);
            var currentUserRoles = await this.userManager.GetRolesAsync(user);
            var currentUserRoleName = currentUserRoles[0];
            IdentityRole newRole = await this.roleManager.FindByNameAsync(inputRole);

            var removeResult = await this.userManager.RemoveFromRoleAsync(user, currentUserRoleName);

            if (removeResult.Succeeded == false)
            {
                return false;
            }

            var addToRoleResult = await this.userManager.AddToRoleAsync(user, newRole.Name);

            return addToRoleResult.Succeeded;
        }
    }
}
