namespace TizianaTerenzi.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Services.Data.Dashboard;

    public class DashboardController : AdministrationController
    {
        private readonly IDashboardService dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await this.dashboardService.GetDashboardInformationAsync();

            return this.View(viewModel);
        }
    }
}
