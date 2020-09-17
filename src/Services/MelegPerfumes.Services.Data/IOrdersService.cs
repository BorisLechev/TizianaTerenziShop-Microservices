namespace MelegPerfumes.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MelegPerfumes.Data.Models;

    public interface IOrdersService
    {
        Task<Order> CreateOrderAsync(string userId, ICollection<OrderProduct> orderProducts);
    }
}
