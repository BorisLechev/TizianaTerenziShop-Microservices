namespace TizianaTerenzi.Orders.Services.Data.Orders
{
    using TizianaTerenzi.Common.Messages.Carts;
    using TizianaTerenzi.Orders.Web.Models.Orders;

    public interface IOrdersService
    {
        Task<bool> OrderAsync(ProductsInTheUserCartHaveBeenOrderedMessage model);

        Task<IEnumerable<OrdersListingViewModel>> GetAllOrdersByUserIdAsync(string userId);

        Task<IEnumerable<OrdersListingViewModel>> GetAllOrdersAsync();

        Task<IEnumerable<OrdersListingViewModel>> GetAllPendingOrdersAsync();

        Task<IEnumerable<OrdersListingViewModel>> GetAllProcessedOrdersAsync();

        Task<IEnumerable<OrderProductsListingViewModel>> GetAllOrderProductsByOrderIdAsync(int orderId);

        Task<bool> ProcessOrderAsync(int orderId);

        Task<bool> DeleteAllOrdersByUserIdAsync(string userId);

        Task DeleteAllOrderProductsByUserIdAsync(string userId);
    }
}
