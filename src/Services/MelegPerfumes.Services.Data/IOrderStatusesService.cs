namespace MelegPerfumes.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MelegPerfumes.Data.Models;

    public interface IOrderStatusesService
    {
        Task<bool> CreateOrderStatusAsync(OrderStatus orderStatus);

        Task CreateOrderStatusesRangeAsync(IEnumerable<OrderStatus> orderStatuses);

        IQueryable<OrderStatus> GetAllOrderStatuses();

        OrderStatus FindByNameAsync(string orderStatusName);
    }
}
