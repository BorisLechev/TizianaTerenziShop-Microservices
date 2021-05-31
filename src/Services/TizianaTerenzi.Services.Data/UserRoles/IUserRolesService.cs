namespace TizianaTerenzi.Services.Data.UserRoles
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.Web.ViewModels.UserRoles;

    public interface IUserRolesService
    {
        Task<IEnumerable<ApplicationUserViewModel>> GetAllUsersAsync();

        Task<UsernamesRolesIndexViewModel> GetUsernamesRolesAsync();

        Task<bool> IsUserAlreadyAddedInRoleAsync(string inputUsername, string inputRole);

        Task<bool> UpdateUserRoleAsync(string username, string inputRole);
    }
}
