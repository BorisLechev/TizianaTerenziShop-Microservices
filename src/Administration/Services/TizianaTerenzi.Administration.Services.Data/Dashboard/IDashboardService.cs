namespace TizianaTerenzi.Administration.Services.Data.Dashboard
{
    using TizianaTerenzi.Administration.Web.Models.Dashboard;

    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardInformationAsync();
    }
}
