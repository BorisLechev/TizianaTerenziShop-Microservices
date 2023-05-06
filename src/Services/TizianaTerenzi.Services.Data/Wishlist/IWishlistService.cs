namespace TizianaTerenzi.Services.Data.Wishlist
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.WebClient.ViewModels.Products;

    public interface IWishlistService
    {
        Task<IEnumerable<WishlistViewModel>> GetAllProductsFromUsersWishlistAsync(string userId);

        Task<bool> AddProductToTheWishlistAsync(int productId, string userId);

        Task<bool> DeleteProductFromTheWishlistAsync(int productId, string userId);

        Task<bool> DeleteAllProductsInTheWishlistAsync(string userId);

        Task<bool> HasTheProductAlreadyAddedToTheWishlistAsync(int productId, string userId);
    }
}
