namespace TizianaTerenzi.Services.Data.Dashboard
{
    using System.Threading.Tasks;

    using TizianaTerenzi.Web.ViewModels.Dashboard;

    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardInformationAsync();

        Task<UsernamesRolesIndexViewModel> GetUsernamesRolesAsync();

        Task<bool> IsUserAlreadyAddedInRoleAsync(string inputUsername, string inputRole);

        Task<bool> UpdateUserRoleAsync(string username, string inputRole);

        Task<bool> DeleteUserInRoleAsync(string userId);
    }
}
