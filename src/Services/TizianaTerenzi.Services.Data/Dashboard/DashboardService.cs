namespace TizianaTerenzi.Services.Data.Dashboard
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Web.ViewModels.Dashboard;
    using Microsoft.AspNetCore.Identity;
    using TizianaTerenzi.Services.Data.Statistics;

    public class DashboardService : IDashboardService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        private readonly IDeletableEntityRepository<ApplicationRole> rolesRepository;

        private readonly IDeletableEntityRepository<Product> productsRepository;

        private readonly IDeletableEntityRepository<Order> ordersRepository;

        private readonly IDeletableEntityRepository<OrderProduct> orderProductsRepository;

        private readonly IStatisticsService statisticsService;

        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly RoleManager<ApplicationRole> roleManager;

        public DashboardService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<ApplicationRole> rolesRepository,
            IDeletableEntityRepository<Product> productsRepository,
            IDeletableEntityRepository<Order> ordersRepository,
            IDeletableEntityRepository<OrderProduct> orderProductsRepository,
            IStatisticsService statisticsService,
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this.usersRepository = usersRepository;
            this.rolesRepository = rolesRepository;
            this.productsRepository = productsRepository;
            this.ordersRepository = ordersRepository;
            this.orderProductsRepository = orderProductsRepository;
            this.statisticsService = statisticsService;
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
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

            var ordersValue = await this.statisticsService.GetTheValueOfAllSalesForTheLast10DaysAsync();

            var orderedProductsCountForThisMonth = await this.statisticsService
                .GetNumberOfPurchasesForEachProductForTheCurrentMonthAsync();

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
    }
}
