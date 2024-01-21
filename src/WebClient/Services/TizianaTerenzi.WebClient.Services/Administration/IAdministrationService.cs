namespace TizianaTerenzi.WebClient.Services.Administration
{
    using Refit;
    using TizianaTerenzi.WebClient.ViewModels.Dashboard;

    public interface IAdministrationService
    {
        [Get("/Dashboard/Index")]
        Task<DashboardViewModel> GetDashboardInformationAsync();
    }
}
