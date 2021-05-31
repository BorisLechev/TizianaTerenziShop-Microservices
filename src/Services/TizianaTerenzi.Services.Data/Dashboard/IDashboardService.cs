namespace TizianaTerenzi.Services.Data.Dashboard
{
    using System.Threading.Tasks;

    using TizianaTerenzi.Web.ViewModels.Dashboard;

    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardInformationAsync();
    }
}
