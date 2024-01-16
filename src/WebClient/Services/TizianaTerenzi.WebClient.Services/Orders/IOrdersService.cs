namespace TizianaTerenzi.WebClient.Services.Orders
{
    using Refit;
    using TizianaTerenzi.WebClient.ViewModels.Orders;

    public interface IOrdersService
    {
        [Get("/Orders/Index")]
        Task<IEnumerable<OrdersListingViewModel>> GetAllOrdersByUserId(string userId);

        [Get("/Orders/Products")]
        Task<IEnumerable<OrderProductsListingViewModel>> GetAllOrderProductsByOrderId(int orderId);
    }
}
