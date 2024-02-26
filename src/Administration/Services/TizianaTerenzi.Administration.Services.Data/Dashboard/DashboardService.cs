namespace TizianaTerenzi.Administration.Services.Data.Dashboard
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Administration.Web.Models.Dashboard;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Data.Repositories;

    public class DashboardService : IDashboardService
    {
        private readonly IDeletableEntityRepository<UserStatistics> userStatisticsRepository;
        private readonly IDeletableEntityRepository<OrderStatistics> orderStatisticsRepository;
        private readonly IDeletableEntityRepository<OrderProductStatistics> orderProductStatisticsRepository;
        private readonly IDictionary<DateTime, decimal> dictionary;

        public DashboardService(
            IDeletableEntityRepository<UserStatistics> userStatisticsRepository,
            IDeletableEntityRepository<OrderStatistics> orderStatisticsRepository,
            IDeletableEntityRepository<OrderProductStatistics> orderProductStatisticsRepository)
        {
            this.userStatisticsRepository = userStatisticsRepository;
            this.orderStatisticsRepository = orderStatisticsRepository;
            this.orderProductStatisticsRepository = orderProductStatisticsRepository;
            this.dictionary = new Dictionary<DateTime, decimal>();
        }

        public async Task<DashboardViewModel> GetDashboardInformationAsync()
        {
            var usersCount = await this.userStatisticsRepository
                                .AllAsNoTracking()
                                .CountAsync();

            var adminsCount = await this.userStatisticsRepository
                                    .AllAsNoTracking()
                                    .CountAsync(u => u.RoleName == GlobalConstants.AdministratorRoleName);

            var totalBannedUsersCount = await this.userStatisticsRepository
                                            .AllAsNoTrackingWithDeleted()
                                            .CountAsync(u => u.IsBlocked);

            var orderedProductsCountForTheCurrentMonth = await this.orderProductStatisticsRepository
                                                                .AllAsNoTracking()
                                                                .CountAsync(op => op.CreatedOn.Month == DateTime.UtcNow.Month);

            var numberOfPurchasesForTheCurrentMonth = await this.orderStatisticsRepository
                                                    .AllAsNoTracking()
                                                    .CountAsync(o => o.CreatedOn.Month == DateTime.UtcNow.Month);

            var totalRevenueForTheCurrentMonth = await this.orderProductStatisticsRepository
                                                        .AllAsNoTracking()
                                                        .Where(op => op.CreatedOn.Month == DateTime.UtcNow.Month)
                                                        .SumAsync(op => op.Price * op.Quantity);

            var totalRevenueForTheCurrentYear = await this.orderProductStatisticsRepository
                                                        .AllAsNoTracking()
                                                        .Where(op => op.CreatedOn.Year == DateTime.UtcNow.Year)
                                                        .SumAsync(op => op.Price * op.Quantity);

            var ordersValue = await this.GetTheValueOfAllSalesForTheLast10DaysAsync();

            var orderedProductsCountForThisMonth = await this.GetNumberOfPurchasesForEachProductForTheCurrentMonthAsync();

            var viewModel = new DashboardViewModel
            {
                TotalUsersCount = usersCount,
                TotalUsersInAdminRoleCount = adminsCount,
                TotalBannedUsersCount = totalBannedUsersCount,
                TotalOrderedProductsCountForTheCurrentMonth = orderedProductsCountForTheCurrentMonth,
                TotalOrdersCountForTheCurrentMonth = numberOfPurchasesForTheCurrentMonth,
                TotalRevenueForTheCurrentMonth = totalRevenueForTheCurrentMonth,
                TotalRevenueForTheCurrentYear = totalRevenueForTheCurrentYear,
                SalesValueFromTheLast10Days = ordersValue,
                NumberOfPurchasesForEachProductForThisMonth = orderedProductsCountForThisMonth,
            };

            return viewModel;
        }

        private async Task<IDictionary<DateTime, decimal>> GetTheValueOfAllSalesForTheLast10DaysAsync()
        {
            var dic = this.CreateEmptyDictionaryWithDateTimeDecimalFor10Days();

            var orderProducts = await this.orderProductStatisticsRepository
                                    .AllAsNoTracking()
                                    .Where(o => o.CreatedOn > DateTime.UtcNow.Date.AddDays(-10))
                                    .GroupBy(o => o.CreatedOn.Date)
                                    .Select(o => new
                                    {
                                        Day = o.Key,
                                        Value = o.Sum(p => p.Price * p.Quantity),
                                    })
                                    .ToDictionaryAsync(o => o.Day, o => o.Value);

            foreach (var product in orderProducts)
            {
                if (dic.ContainsKey(product.Key))
                {
                    dic[product.Key.Date] = product.Value;
                }
            }

            return dic;
        }

        private async Task<IEnumerable<GroupByViewModel<string, int>>> GetNumberOfPurchasesForEachProductForTheCurrentMonthAsync()
        {
            var orderProducts = await this.orderProductStatisticsRepository
                                    .AllAsNoTracking()
                                    .Where(op => op.CreatedOn.Month == DateTime.UtcNow.Month)
                                    .GroupBy(op => op.ProductName)
                                    .Select(p => new GroupByViewModel<string, int>
                                    {
                                        Group = p.Key,
                                        Count = p.Sum(x => x.Quantity),
                                    })
                                    .ToListAsync();

            return orderProducts;
        }

        private IDictionary<DateTime, decimal> CreateEmptyDictionaryWithDateTimeDecimalFor10Days()
        {
            var startDate = DateTime.UtcNow.Date.AddDays(-9);

            for (DateTime i = startDate; i <= DateTime.UtcNow.Date; i = i.AddDays(1))
            {
                this.dictionary.Add(i.Date, 0);
            }

            return this.dictionary;
        }
    }
}
