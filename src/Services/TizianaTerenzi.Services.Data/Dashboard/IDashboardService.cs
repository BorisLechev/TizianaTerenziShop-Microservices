namespace TizianaTerenzi.Services.Data.Dashboard
{
    using System.Threading.Tasks;

    using TizianaTerenzi.WebClient.ViewModels.Dashboard;

    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardInformationAsync();
    }
}
