namespace TizianaTerenzi.WebClient.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.WebClient.Services.Administration;

    public class DashboardController : AdministrationController
    {
        private readonly IAdministrationService administrationService;

        public DashboardController(IAdministrationService administrationService)
        {
            this.administrationService = administrationService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await this.administrationService.GetDashboardInformationAsync();

            return this.View(viewModel);
        }
    }
}
