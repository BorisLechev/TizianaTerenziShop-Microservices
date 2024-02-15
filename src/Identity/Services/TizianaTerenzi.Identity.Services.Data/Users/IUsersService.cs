namespace TizianaTerenzi.Identity.Services.Data.Users
{
    using TizianaTerenzi.Identity.Web.Models.Users;

    public interface IUsersService
    {
        Task<IEnumerable<ApplicationUserViewModel>> GetAllUsersAsync();

        Task<IEnumerable<BannedApplicationUserViewModel>> GetAllBannedUsersAsync();

        Task<UsernamesRolesIndexViewModel> GetUsernamesRolesAsync();

        Task<bool> AddUserInRole(string userId, string inputRoleId);

        Task<bool> IsUserAlreadyAddedInRoleAsync(string inputUserId, string inputRoleId);

        Task<bool> UpdateUserRoleAsync(string userId, string inputRoleId);
    }
}
