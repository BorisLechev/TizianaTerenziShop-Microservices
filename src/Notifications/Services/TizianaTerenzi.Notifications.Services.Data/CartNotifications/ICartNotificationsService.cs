namespace TizianaTerenzi.Notifications.Services.Data.CartNotifications
{
    using TizianaTerenzi.Common.Messages.Carts;

    public interface ICartNotificationsService
    {
        Task<bool> AddCartNotificationAsync(string userId, int numberOfProductsInTheUsersCart);

        Task<int> GetNumberOfProductsInTheUsersCartAsync(string userId);

        Task<bool> DeleteAllProductsInTheCartByUserIdAsync(ProductsQuantityInTheUsersCartDeletedMessage message);

        Task<bool> IncreaseQuantityAsync(ProductsQuantityInTheUsersCartIncreasedMessage message);

        Task<bool> ReduceQuantityAsync(ProductsQuantityInTheUsersCartReducedMessage message);
    }
}
