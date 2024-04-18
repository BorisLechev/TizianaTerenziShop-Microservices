namespace TizianaTerenzi.Administration.Services.Data.Orders
{
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Administration;
    using TizianaTerenzi.Common.Messages.Orders;
    using TizianaTerenzi.Common.Services.ServiceRegistrationAttributes;

    [TransientRegistration]
    public class OrdersService : IOrdersService
    {
        private readonly IDeletableEntityRepository<OrderStatistics> orderStatisticsRepository;

        public OrdersService(
            IDeletableEntityRepository<OrderStatistics> orderStatisticsRepository)
        {
            this.orderStatisticsRepository = orderStatisticsRepository;
        }

        public async Task<bool> AddOrderStatisticsAsync(OrderAddedInAdminStatisticsMessage model)
        {
            var orderProducts = model.Products
                            .Select(p => new OrderProductStatistics
                            {
                                ProductName = p.ProductName,
                                Price = p.PriceWithDiscountCode,
                                Quantity = p.Quantity,
                            })
                            .ToList();

            var order = new OrderStatistics
            {
                OrderId = model.OrderId,
                Products = orderProducts,
            };

            await this.orderStatisticsRepository.AddAsync(order);
            var result = await this.orderStatisticsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task ProcessOrderAsync(int orderId)
        {
            var message = new OrderProcessedMessage
            {
                OrderId = orderId,
            };

            await this.orderStatisticsRepository.SaveAndPublishEventMessageAsync(message);
        }
    }
}
