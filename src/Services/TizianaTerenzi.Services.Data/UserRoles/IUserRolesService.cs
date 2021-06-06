namespace TizianaTerenzi.Services.Data.UserRoles
{
    using System.Threading.Tasks;

    using TizianaTerenzi.Web.ViewModels.UserRoles;

    public interface IUserRolesService
    {
        Task<AllUsersViewModel> GetAllUsersAsync();

        Task<AllUsersViewModel> GetAllBannedUsersAsync();

        Task<UsernamesRolesIndexViewModel> GetUsernamesRolesAsync();

        Task<bool> IsUserAlreadyAddedInRoleAsync(string inputUsername, string inputRole);

        Task<bool> UpdateUserRoleAsync(string username, string inputRole);
    }
}
