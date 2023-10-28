namespace TizianaTerenzi.Notifications.Services.Carts
{
    using Refit;

    public interface ICartsService
    {
        [Get("/Carts/GetNumberOfProductsInTheUsersCart")]
        Task<int> GetNumberOfProductsInTheUsersCart();
    }
}
