namespace TizianaTerenzi.Products.Services.Data.Wishlist
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IWishlistService
    {
        Task<IEnumerable<T>> GetAllProductsFromUsersWishlistAsync<T>(string userId);

        Task<bool> AddProductToTheWishlistAsync(int productId, string userId);

        Task<bool> DeleteProductFromTheWishlistAsync(int productId, string userId);

        Task<bool> DeleteAllProductsInTheWishlistAsync(string userId);

        Task<bool> HasTheProductAlreadyAddedToTheWishlistAsync(int productId, string userId);
    }
}
