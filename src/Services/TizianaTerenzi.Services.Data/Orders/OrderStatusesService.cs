namespace TizianaTerenzi.Services.Data.Orders
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;

    public class OrderStatusesService : IOrderStatusesService
    {
        private readonly IDeletableEntityRepository<OrderStatus> orderStatusesRepository;

        public OrderStatusesService(IDeletableEntityRepository<OrderStatus> orderStatusesRepository)
        {
            this.orderStatusesRepository = orderStatusesRepository;
        }

        public async Task<bool> CreateOrderStatusAsync(OrderStatus orderStatus)
        {
            await this.orderStatusesRepository.AddAsync(orderStatus);

            int result = await this.orderStatusesRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<OrderStatus> FindByNameAsync(string orderStatusName)
        {
            var orderStatus = await this.orderStatusesRepository
                .All()
                .SingleOrDefaultAsync(os => os.Name == orderStatusName);

            return orderStatus;
        }

        public async Task<IEnumerable<OrderStatus>> GetAllOrderStatusesAsync()
        {
            var orderStatuses = await this.orderStatusesRepository
                .AllAsNoTracking()
                .ToListAsync();

            return orderStatuses;
        }
    }
}
