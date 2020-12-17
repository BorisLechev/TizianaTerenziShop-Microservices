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

        Task<IEnumerable<OrdersListingViewModel>> GetAllOrdersByUserAsync(string userName);

        Task<IEnumerable<OrderProductsListingViewModel>> GetAllOrderProductsAsync(int orderId);

        Task<IEnumerable<OrderProductsListingViewModel>> GetAllOrderProductsByUserAsync(string userName, int orderId);

        Task<bool> ProcessOrderAsync(int orderId);
    }
}
