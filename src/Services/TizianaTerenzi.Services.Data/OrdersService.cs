namespace TizianaTerenzi.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.Web.ViewModels.Orders;

    public class OrdersService : IOrdersService
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IDeletableEntityRepository<Order> ordersRepository;

        private readonly IDeletableEntityRepository<OrderProduct> orderProductsRepository;

        private readonly IOrderStatusesService orderStatusesService;

        public OrdersService(
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<Order> ordersRepository,
            IDeletableEntityRepository<OrderProduct> orderProductsRepository,
            IOrderStatusesService orderStatusesService)
        {
            this.userManager = userManager;
            this.ordersRepository = ordersRepository;
            this.orderProductsRepository = orderProductsRepository;
            this.orderStatusesService = orderStatusesService;
        }

        public async Task<IEnumerable<OrdersListingViewModel>> GetAllOrdersByUserAsync(string userName)
        {
            if (userName == null)
            {
                return null;
            }

            var user = await this.userManager.FindByNameAsync(userName);

            var ordersByUser = await this.ordersRepository
                .All()
                .Where(op => op.UserId == user.Id)
                .To<OrdersListingViewModel>()
                .ToListAsync();

            return ordersByUser;
        }

        public async Task<IEnumerable<OrderProductsListingViewModel>> GetAllOrderProductsByUserAsync(string userName, int orderId)
        {
            if (userName == null)
            {
                return null;
            }

            var user = await this.userManager.FindByNameAsync(userName);

            var orderProductsByUser = await this.orderProductsRepository
                .All()
                .Where(op => op.UserId == user.Id && op.OrderId == orderId)
                .To<OrderProductsListingViewModel>()
                .ToListAsync();

            return orderProductsByUser;
        }

        public async Task<IEnumerable<OrdersListingViewModel>> GetAllOrdersAsync()
        {
            var orders = await this.ordersRepository
                .All()
                .To<OrdersListingViewModel>()
                .ToListAsync();

            return orders;
        }

        public async Task<IEnumerable<OrdersListingViewModel>> GetAllPendingOrdersAsync()
        {
            var orders = await this.ordersRepository
                .All()
                .Where(o => o.Status.Name == "Pending")
                .To<OrdersListingViewModel>()
                .ToListAsync();

            return orders;
        }

        public async Task<IEnumerable<OrdersListingViewModel>> GetAllProcessedOrdersAsync()
        {
            var orders = await this.ordersRepository
                .All()
                .Where(o => o.Status.Name == "Completed")
                .To<OrdersListingViewModel>()
                .ToListAsync();

            return orders;
        }

        public async Task<IEnumerable<OrderProductsListingViewModel>> GetAllOrderProductsAsync(int orderId)
        {
            var orderProducts = await this.orderProductsRepository
                .All()
                .Where(op => op.OrderId == orderId)
                .To<OrderProductsListingViewModel>()
                .ToListAsync();

            return orderProducts;
        }

        public async Task<bool> ProcessOrderAsync(int orderId)
        {
            var order = await this.ordersRepository
                .All()
                .SingleOrDefaultAsync(o => o.Id == orderId);

            order.StatusId = this.orderStatusesService.FindByNameAsync("Completed").Id;
            var result = await this.ordersRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
