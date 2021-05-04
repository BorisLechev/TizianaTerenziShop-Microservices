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
            var orderedProductsCountForThisMonth = await this.statisticsService.GetNumberOfPurchasesForEachProductForTheCurrentMonthAsync();
            var totalRevenueForTheCurrentMonth = await this.statisticsService.GetTotalRevenueForTheCurrentMonthAsync();
            var numberOfOrdersForThisMonth = await this.statisticsService.GetNumberOfOrdersForTheCurrentMonthAsync();
            var numberofRegisteredUsers = await this.statisticsService.GetNumberOfRegisteredUsersAsync();

            var viewModel = new StatisticsIndexPageViewModel
            {
                OrdersFromTheLast10Days = orders,
                SalesValueFromTheLast10Days = ordersValue,
                NumberOfPurchasesForEachProductForThisMonth = orderedProductsCountForThisMonth,
                TotalRevenueForTheCurrentMonth = totalRevenueForTheCurrentMonth,
                NumberOfOrdersForTheCurrentMonth = numberOfOrdersForThisMonth,
                NumberOfRegisteredUsers = numberofRegisteredUsers,
            };

            return this.View(viewModel);
        }
    }
}
