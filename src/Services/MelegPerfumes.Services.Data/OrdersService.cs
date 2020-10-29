namespace MelegPerfumes.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MelegPerfumes.Data.Common.Repositories;
    using MelegPerfumes.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class OrdersService : IOrdersService
    {
        private readonly IDeletableEntityRepository<Order> ordersRepository;

        private readonly IDeletableEntityRepository<OrderStatus> orderStatusesRepository;

        private readonly IOrderStatusesService orderStatusesService;

        public OrdersService(
            IDeletableEntityRepository<Order> ordersRepository,
            IDeletableEntityRepository<OrderStatus> orderStatusesRepository,
            IOrderStatusesService orderStatusesService)
        {
            this.ordersRepository = ordersRepository;
            this.orderStatusesRepository = orderStatusesRepository;
            this.orderStatusesService = orderStatusesService;
        }

        public async Task<Order> CreateOrderAsync(string userId, ICollection<OrderProduct> orderProducts)
        {
            var order = new Order
            {
                UserId = userId,
                StatusId = this.orderStatusesService.FindByNameAsync("Pending").Id,
                Products = orderProducts,
            };

            await this.ordersRepository.AddAsync(order);
            await this.ordersRepository.SaveChangesAsync();

            var createdOrder = await this.ordersRepository
                .All()
                .Include(o => o.Products)
                .ThenInclude(o => o.Product)
                .SingleOrDefaultAsync(o => o.Id == order.Id);

            return createdOrder;
        }
    }
}
