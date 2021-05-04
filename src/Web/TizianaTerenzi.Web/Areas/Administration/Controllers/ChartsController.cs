namespace TizianaTerenzi.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Services.Data.Statistics;
    using TizianaTerenzi.Web.ViewModels.Statistics;

    public class ChartsController : AdministrationController
    {
        private readonly IStatisticsService statisticsService;

        public ChartsController(
            IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await this.statisticsService.GetAllOrdersForTheLast10DaysAsync();
            var ordersValue = await this.statisticsService.GetTheValueOfAllSalesForTheLast10DaysAsync();
            var orderedProductsCountForThisMonth = await this.statisticsService.GetNumberOfPurchasesForEachProductForThisMonthAsync();

            var viewModel = new StatisticsIndexPageViewModel
            {
                OrdersFromTheLast10Days = orders,
                SalesValueFromTheLast10Days = ordersValue,
                NumberOfPurchasesForEachProductForThisMonth = orderedProductsCountForThisMonth,
            };

            return this.View(viewModel);
        }
    }
}
