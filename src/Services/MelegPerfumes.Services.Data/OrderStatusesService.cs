namespace MelegPerfumes.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MelegPerfumes.Data.Common.Repositories;
    using MelegPerfumes.Data.Models;
    using Microsoft.EntityFrameworkCore;

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

        public async Task CreateOrderStatusesRangeAsync(IEnumerable<OrderStatus> orderStatuses)
        {
            await this.orderStatusesRepository.AddRangeAsync(orderStatuses);
            await this.orderStatusesRepository.SaveChangesAsync();
        }

        public async Task<OrderStatus> FindByNameAsync(string orderStatusName)
        {
            var orderStatus = await this.orderStatusesRepository
                .All()
                .SingleOrDefaultAsync(os => os.Name == orderStatusName);

            return orderStatus;
        }

        public IQueryable<OrderStatus> GetAllOrderStatuses()
        {
            var orderStatuses = this.orderStatusesRepository.All();

            return orderStatuses;
        }
    }
}
