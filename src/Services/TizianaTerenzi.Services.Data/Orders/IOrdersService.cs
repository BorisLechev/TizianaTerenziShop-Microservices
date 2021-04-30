namespace TizianaTerenzi.Services.Data.Orders
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.Web.ViewModels.Orders;

    public interface IOrdersService
    {
        Task<IEnumerable<OrdersListingViewModel>> GetAllOrdersAsync();

        Task<IEnumerable<OrdersListingViewModel>> GetAllPendingOrdersAsync();

        Task<IEnumerable<OrdersListingViewModel>> GetAllProcessedOrdersAsync();

        Task<IEnumerable<OrdersListingViewModel>> GetAllOrdersByUserIdAsync(string userId);

        Task<IEnumerable<OrderProductsListingViewModel>> GetAllOrderProductsByOrderIdAsync(int orderId);

        Task<bool> ProcessOrderAsync(int orderId);

        Task DeleteAllOrdersByUserIdAsync(string userId);

        Task DeleteAllOrderProductsByUserIdAsync(string userId);
    }
}
