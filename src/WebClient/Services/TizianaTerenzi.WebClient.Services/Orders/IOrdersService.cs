namespace TizianaTerenzi.WebClient.Services.Orders
{
    using Refit;
    using TizianaTerenzi.WebClient.ViewModels.Orders;

    public interface IOrdersService
    {
        [Get("/Orders/Index")]
        Task<IEnumerable<OrdersListingViewModel>> GetAllOrdersByUserIdAsync(string userId);

        [Get("/Orders/Products")]
        Task<IEnumerable<OrderProductsListingViewModel>> GetAllOrderProductsByOrderIdAsync(int orderId);

        [Get("/Orders/All")]
        Task<IEnumerable<OrdersListingViewModel>> GetAllOrdersAsync();

        [Get("/Orders/Pending")]
        Task<IEnumerable<OrdersListingViewModel>> GetAllPendingOrdersAsync();

        [Get("/Orders/Processed")]
        Task<IEnumerable<OrdersListingViewModel>> GetAllProcessedOrdersAsync();
    }
}
