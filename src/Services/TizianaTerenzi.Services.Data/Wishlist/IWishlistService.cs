namespace TizianaTerenzi.Services.Data.Wishlist
{
    using System.Threading.Tasks;

    using TizianaTerenzi.Web.ViewModels.Products;

    public interface IWishlistService
    {
        Task<WishlistViewModel> GetAllProductsFromUserWishlistAsync(string userId);

        Task<bool> AddProductToTheWishlistAsync(int productId, string userId);

        Task<bool> DeleteProductInTheWishlistAsync(int productId, string userId);

        Task DeleteAllProductsInTheWishlistAsync(string userId);

        Task<bool> IsTheProductAlreadyAddedInWishlistAsync(int productId, string userId);
    }
}
