namespace TizianaTerenzi.Services.Data.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Web.ViewModels.Dashboard;

    public class DashboardService : IDashboardService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        private readonly IDeletableEntityRepository<ApplicationRole> rolesRepository;

        private readonly IDeletableEntityRepository<Order> ordersRepository;

        private readonly IDeletableEntityRepository<OrderProduct> orderProductsRepository;

        private readonly IDictionary<DateTime, decimal> dictionary;

        public DashboardService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<ApplicationRole> rolesRepository,
            IDeletableEntityRepository<Order> ordersRepository,
            IDeletableEntityRepository<OrderProduct> orderProductsRepository)
        {
            this.usersRepository = usersRepository;
            this.rolesRepository = rolesRepository;
            this.ordersRepository = ordersRepository;
            this.orderProductsRepository = orderProductsRepository;
            this.dictionary = new Dictionary<DateTime, decimal>();
        }

        public async Task<DashboardViewModel> GetDashboardInformationAsync()
        {
            var usersCount = await this.usersRepository
                .AllAsNoTracking()
                .CountAsync();

            var adminRoleId = await this.rolesRepository
                .AllAsNoTracking()
                .Where(r => r.Name == GlobalConstants.AdministratorRoleName)
                .Select(r => r.Id)
                .SingleOrDefaultAsync();

            var adminsCount = await this.usersRepository
                .AllAsNoTracking()
                .CountAsync(u => u.Roles.Any(r => r.RoleId == adminRoleId));

            var orderedProductsCountForTheCurrentMonth = await this.orderProductsRepository
                .AllAsNoTracking()
                .CountAsync(op => op.CreatedOn.Month == DateTime.UtcNow.Month);

            var numberOfPurchasesForTheCurrentMonth = await this.ordersRepository
                                                    .AllAsNoTracking()
                                                    .CountAsync(o => o.CreatedOn.Month == DateTime.UtcNow.Month);

            var totalRevenueForTheCurrentMonth = await this.orderProductsRepository
                                                .AllAsNoTracking()
                                                .Where(op => op.CreatedOn.Month == DateTime.UtcNow.Month)
                                                .SumAsync(op => op.Price);

            var totalRevenueForTheCurrentYear = await this.orderProductsRepository
                                                .AllAsNoTracking()
                                                .Where(op => op.CreatedOn.Year == DateTime.UtcNow.Year)
                                                .SumAsync(op => op.Price);

            var ordersValue = await this.GetTheValueOfAllSalesForTheLast10DaysAsync();

            var orderedProductsCountForThisMonth = await this.GetNumberOfPurchasesForEachProductForTheCurrentMonthAsync();

            var viewModel = new DashboardViewModel
            {
                TotalUsersCount = usersCount,
                TotalUsersInAdminRoleCount = adminsCount,
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

            var orderProducts = await this.orderProductsRepository
                .AllAsNoTracking()
                .Where(o => o.CreatedOn > DateTime.UtcNow.Date.AddDays(-10))
                .GroupBy(o => o.CreatedOn.Date)
                .Select(o => new
                {
                    Day = o.Key,
                    Value = o.Sum(p => p.Price),
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
            var orderProducts = await this.orderProductsRepository
                .AllAsNoTracking()
                .Where(op => op.CreatedOn.Month == DateTime.UtcNow.Month)
                .GroupBy(op => op.Product.Name)
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
