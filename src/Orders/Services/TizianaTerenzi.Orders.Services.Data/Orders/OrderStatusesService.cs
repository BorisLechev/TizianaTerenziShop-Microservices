namespace TizianaTerenzi.Orders.Services.Data.Orders
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Services.ServiceRegistrationAttributes;
    using TizianaTerenzi.Orders.Data.Models;

    [TransientRegistration]
    public class OrderStatusesService : IOrderStatusesService
    {
        private readonly IDeletableEntityRepository<OrderStatus> orderStatusesRepository;

        public OrderStatusesService(IDeletableEntityRepository<OrderStatus> orderStatusesRepository)
        {
            this.orderStatusesRepository = orderStatusesRepository;
        }

        public async Task<int> FindByNameAsync(string orderStatusName)
        {
            var orderStatusId = await this.orderStatusesRepository
                .AllAsNoTracking()
                .Where(os => os.Name == orderStatusName)
                .Select(os => os.Id)
                .SingleOrDefaultAsync();

            return orderStatusId;
        }
    }
}
