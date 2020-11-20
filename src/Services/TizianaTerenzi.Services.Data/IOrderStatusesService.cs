namespace TizianaTerenzi.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;

    public interface IOrderStatusesService
    {
        Task<bool> CreateOrderStatusAsync(OrderStatus orderStatus);

        Task CreateOrderStatusesRangeAsync(IEnumerable<OrderStatus> orderStatuses);

        IQueryable<OrderStatus> GetAllOrderStatuses();

        Task<OrderStatus> FindByNameAsync(string orderStatusName);
    }
}
