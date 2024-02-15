namespace TizianaTerenzi.Identity.Services.Data.Users
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Identity.Data.Models;
    using TizianaTerenzi.Identity.Web.Models.Users;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<ApplicationRole> rolesRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<ApplicationRole> rolesRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this.usersRepository = usersRepository;
            this.rolesRepository = rolesRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IEnumerable<ApplicationUserViewModel>> GetAllUsersAsync()
        {
            var allUsers = await this.usersRepository
                                .AllAsNoTracking()
                                .ToListAsync();

            var allUsersViewModel = new List<ApplicationUserViewModel>();

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

                allUsersViewModel.Add(viewModel);
            }

            return allUsersViewModel;
        }

        public async Task<IEnumerable<BannedApplicationUserViewModel>> GetAllBannedUsersAsync()
        {
            var allBannedUsers = await this.usersRepository
                                       .AllWithDeleted()
                                       .Where(u => u.IsBlocked)
                                       .To<BannedApplicationUserViewModel>()
                                       .ToListAsync();

            return allBannedUsers;
        }

        public async Task<UsernamesRolesIndexViewModel> GetUsernamesRolesAsync()
        {
            var users = await this.usersRepository
                            .All()
                            .Select(u => new SelectListItem
                            {
                                Value = u.Id,
                                Text = u.UserName,
                            })
                            .ToListAsync();

            var roles = await this.rolesRepository
                            .All()
                            .Select(r => new SelectListItem
                            {
                                Value = r.Id,
                                Text = r.Name,
                            })
                            .ToListAsync();

            var viewModel = new UsernamesRolesIndexViewModel
            {
                Users = users,
                Roles = roles,
            };

            return viewModel;
        }

        public async Task<bool> AddUserInRole(string inputUserId, string inputRoleId)
        {
            var isUserAlreadyAddedInThisRole = await this.IsUserAlreadyAddedInRoleAsync(inputUserId, inputRoleId);

            if (isUserAlreadyAddedInThisRole)
            {
                return false;
            }
            else
            {
                var result = await this.UpdateUserRoleAsync(inputUserId, inputRoleId);

                if (result)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> IsUserAlreadyAddedInRoleAsync(string inputUserId, string inputRoleId)
        {
            var user = await this.userManager.FindByIdAsync(inputUserId);
            IdentityRole newRole = await this.roleManager.FindByIdAsync(inputRoleId);

            if (user == null || newRole == null)
            {
                return false;
            }

            bool isUserAlreadyAddedInRole = await this.userManager.IsInRoleAsync(user, newRole.Name);

            return isUserAlreadyAddedInRole;
        }

        public async Task<bool> UpdateUserRoleAsync(string userId, string inputRoleId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            var currentUserRoles = await this.userManager.GetRolesAsync(user);
            string currentUserRoleName = currentUserRoles[0];
            IdentityRole newRole = await this.roleManager.FindByIdAsync(inputRoleId);

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
