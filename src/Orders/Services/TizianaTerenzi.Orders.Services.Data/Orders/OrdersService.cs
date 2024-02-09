namespace TizianaTerenzi.Orders.Services.Data.Orders
{
    using System.Collections.Generic;

    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Carts;
    using TizianaTerenzi.Common.Messages.Orders;
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Orders.Data.Models;
    using TizianaTerenzi.Orders.Web.Models.Orders;
    using Z.EntityFramework.Plus;

    public class OrdersService : IOrdersService
    {
        private readonly IDeletableEntityRepository<Order> ordersRepository;
        private readonly IDeletableEntityRepository<OrderProduct> orderProductsRepository;
        private readonly IOrderStatusesService orderStatusesService;
        private readonly IBus publisher;

        public OrdersService(
            IDeletableEntityRepository<Order> ordersRepository,
            IDeletableEntityRepository<OrderProduct> orderProductsRepository,
            IOrderStatusesService orderStatusesService,
            IBus publisher)
        {
            this.ordersRepository = ordersRepository;
            this.orderProductsRepository = orderProductsRepository;
            this.orderStatusesService = orderStatusesService;
            this.publisher = publisher;
        }

        public async Task<bool> OrderAsync(ProductsInTheUserCartHaveBeenOrderedMessage model)
        {
            var orderProducts = model.Products
                .Select(p => new OrderProduct
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Quantity = p.Quantity,
                })
                .ToList();

            var orderProduct = model.Products.FirstOrDefault();
            var discountCodeId = orderProduct?.DiscountCodeId;
            var discountCodeName = orderProduct?.DiscountCodeName;
            var discountCodeDiscount = orderProduct?.DiscountCodeDiscount;

            var pendingStatusId = await this.orderStatusesService.FindByNameAsync("Pending");

            var order = new Order
            {
                UserId = model.UserId,
                UserFullName = model.FullName,
                UserEmail = model.Email,
                UserPhoneNumber = model.PhoneNumber,
                UserShippingAddress = model.ShippingAddress,
                UserTown = model.Town,
                UserPostalCode = model.PostalCode,
                UserCountry = model.Country,
                StatusId = pendingStatusId,
                Products = orderProducts,
                DiscountCodeId = discountCodeId,
                CartDiscountCodeValue = discountCodeDiscount,
                CartDiscountCodeName = discountCodeName,
            };

            await this.ordersRepository.AddAsync(order);
            var result = await this.ordersRepository.SaveChangesAsync();

            await this.publisher.Publish(new OrderAddedInAdminStatisticsMessage
            {
                OrderId = order.Id,
                Products = orderProducts.Select(p => new OrderedProductsAddedInAdminStatisticsMessage
                {
                    ProductName = p.ProductName,
                    Quantity = p.Quantity,
                    PriceWithDiscountCode = p.Price,
                }),
            });

            return result > 0;
        }

        public async Task<IEnumerable<OrdersListingViewModel>> GetAllOrdersByUserIdAsync(string userId)
        {
            var ordersByUser = await this.ordersRepository
                                    .AllAsNoTracking()
                                    .Where(o => o.UserId == userId)
                                    .OrderByDescending(o => o.CreatedOn)
                                    .To<OrdersListingViewModel>()
                                    .ToListAsync();

            return ordersByUser;
        }

        public async Task<IEnumerable<OrdersListingViewModel>> GetAllOrdersAsync()
        {
            var orders = await this.ordersRepository
                                .AllAsNoTracking()
                                .OrderByDescending(o => o.CreatedOn)
                                .To<OrdersListingViewModel>()
                                .ToListAsync();

            return orders;
        }

        public async Task<IEnumerable<OrdersListingViewModel>> GetAllPendingOrdersAsync()
        {
            var orders = await this.ordersRepository
                                .AllAsNoTracking()
                                .Where(o => o.Status.Name == "Pending")
                                .OrderByDescending(o => o.CreatedOn)
                                .To<OrdersListingViewModel>()
                                .ToListAsync();

            return orders;
        }

        public async Task<IEnumerable<OrdersListingViewModel>> GetAllProcessedOrdersAsync()
        {
            var orders = await this.ordersRepository
                                .AllAsNoTracking()
                                .Where(o => o.Status.Name == "Completed")
                                .OrderByDescending(o => o.CreatedOn)
                                .To<OrdersListingViewModel>()
                                .ToListAsync();

            return orders;
        }

        public async Task<IEnumerable<OrderProductsListingViewModel>> GetAllOrderProductsByOrderIdAsync(int orderId)
        {
            var orders = await this.orderProductsRepository
                        .AllAsNoTracking()
                        .Where(o => o.OrderId == orderId)
                        .To<OrderProductsListingViewModel>()
                        .ToListAsync();

            return orders;
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

        public async Task DeleteAllOrderProductsByUserIdAsync(string userId)
        {
            var orderProducts = await this.orderProductsRepository
                                    .All()
                                    .Where(op => op.Order.UserId == userId)
                                    .ToArrayAsync();

            if (orderProducts.Any())
            {
                this.orderProductsRepository.DeleteRange(orderProducts);
                await this.orderProductsRepository.SaveChangesAsync();
            }
        }
    }
}
