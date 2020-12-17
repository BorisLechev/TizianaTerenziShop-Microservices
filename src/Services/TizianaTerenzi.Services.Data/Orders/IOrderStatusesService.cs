namespace TizianaTerenzi.Services.Data.Orders
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;

    public interface IOrderStatusesService
    {
        Task<bool> CreateOrderStatusAsync(OrderStatus orderStatus);

        Task<IEnumerable<OrderStatus>> GetAllOrderStatusesAsync();

        Task<OrderStatus> FindByNameAsync(string orderStatusName);
    }
}
