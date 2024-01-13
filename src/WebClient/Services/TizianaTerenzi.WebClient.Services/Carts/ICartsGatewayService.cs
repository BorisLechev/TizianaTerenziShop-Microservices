namespace TizianaTerenzi.WebClient.Services.Carts
{
    using Refit;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.WebClient.ViewModels.Orders;

    public interface ICartsGatewayService
    {
        [Get("/Carts/Checkout")]
        Task<Result<OrderCheckoutViewModel>> Checkout();
    }
}
