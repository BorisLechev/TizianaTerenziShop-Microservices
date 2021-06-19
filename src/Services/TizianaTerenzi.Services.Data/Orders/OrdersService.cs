namespace TizianaTerenzi.Services.Data.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.Web.ViewModels.Orders;
    using Z.EntityFramework.Plus;

    public class OrdersService : IOrdersService
    {
        private readonly IDeletableEntityRepository<Order> ordersRepository;

        private readonly IDeletableEntityRepository<OrderProduct> orderProductsRepository;

        private readonly IOrderStatusesService orderStatusesService;

        public OrdersService(
            IDeletableEntityRepository<Order> ordersRepository,
            IDeletableEntityRepository<OrderProduct> orderProductsRepository,
            IOrderStatusesService orderStatusesService)
        {
            this.ordersRepository = ordersRepository;
            this.orderProductsRepository = orderProductsRepository;
            this.orderStatusesService = orderStatusesService;
        }

        public async Task<IEnumerable<OrdersListingViewModel>> GetAllOrdersByUserIdAsync(string userId)
        {
            var ordersByUser = await this.ordersRepository
                .AllAsNoTracking()
                .Where(op => op.UserId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .To<OrdersListingViewModel>()
                .ToListAsync();

            return ordersByUser;
        }

        public async Task<IEnumerable<OrdersListingViewModel>> GetAllOrdersAsync()
        {
            var orders = await this.ordersRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .To<OrdersListingViewModel>()
                .ToListAsync();

            return orders;
        }

        public async Task<IEnumerable<OrdersListingViewModel>> GetAllPendingOrdersAsync()
        {
            var orders = await this.ordersRepository
                .AllAsNoTracking()
                .Where(o => o.Status.Name == "Pending")
                .OrderByDescending(x => x.CreatedOn)
                .To<OrdersListingViewModel>()
                .ToListAsync();

            return orders;
        }

        public async Task<IEnumerable<OrdersListingViewModel>> GetAllProcessedOrdersAsync()
        {
            var orders = await this.ordersRepository
                .AllAsNoTracking()
                .Where(o => o.Status.Name == "Completed")
                .OrderByDescending(x => x.CreatedOn)
                .To<OrdersListingViewModel>()
                .ToListAsync();

            return orders;
        }

        public async Task<IEnumerable<OrderProductsListingViewModel>> GetAllOrderProductsByOrderIdAsync(int orderId)
        {
            var orderProducts = await this.orderProductsRepository
                .AllAsNoTracking()
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

            var orderStatusId = await this.orderStatusesService.FindByNameAsync("Completed");

            order.StatusId = orderStatusId;
            var result = await this.ordersRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteAllOrdersByUserIdAsync(string userId)
        {
            var ordersCount = await this.ordersRepository
                    .All()
                    .Where(o => o.UserId == userId)
                    .UpdateAsync(o => new Order
                    {
                        IsDeleted = true,
                        DeletedOn = DateTime.UtcNow,
                    });

            return ordersCount > 0;
        }

        public async Task<bool> DeleteAllOrderProductsByUserIdAsync(string userId)
        {
            var deletedProductsCount = await this.orderProductsRepository
                    .All()
                    .Where(op => op.Order.UserId == userId)
                    .UpdateAsync(o => new Order
                    {
                        IsDeleted = true,
                        DeletedOn = DateTime.UtcNow,
                    });

            return deletedProductsCount > 0;
        }
    }
}
