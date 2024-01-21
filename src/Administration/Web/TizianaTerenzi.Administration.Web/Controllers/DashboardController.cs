namespace TizianaTerenzi.Administration.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Administration.Services.Data.Dashboard;
    using TizianaTerenzi.Administration.Web.Models.Dashboard;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Common.Web.ValidationAttributes;

    [AuthorizeAdministratorAttribute]
    public class DashboardController : ApiController
    {
        private readonly IDashboardService dashboardService;

        public DashboardController(
            IDashboardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardViewModel>> Index()
        {
            var dashboardData = await this.dashboardService.GetDashboardInformationAsync();

            return this.Ok(dashboardData);
        }
    }
}
