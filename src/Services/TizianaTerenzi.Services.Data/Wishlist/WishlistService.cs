namespace TizianaTerenzi.Services.Data.Wishlist
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.Web.ViewModels.Products;
    using TizianaTerenzi.Web.ViewModels.Wishlist;

    public class WishlistService : IWishlistService
    {
        private readonly IDeletableEntityRepository<FavoriteProduct> favoriteProductsRepository;

        public WishlistService(IDeletableEntityRepository<FavoriteProduct> favoriteProductsRepository)
        {
            this.favoriteProductsRepository = favoriteProductsRepository;
        }

        public async Task<bool> AddProductToTheWishlistAsync(int productId, string userId)
        {
            var product = new FavoriteProduct
            {
                ProductId = productId,
                UserId = userId,
            };

            await this.favoriteProductsRepository.AddAsync(product);
            int result = await this.favoriteProductsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task DeleteAllProductsInTheWishlistAsync(string userId)
        {
            var products = await this.favoriteProductsRepository
                .All()
                .Where(fp => fp.UserId == userId)
                .ToListAsync();

            this.favoriteProductsRepository.DeleteRange(products);
            await this.favoriteProductsRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteProductInTheWishlistAsync(int productId, string userId)
        {
            var product = await this.favoriteProductsRepository
                .All()
                .Where(fp => fp.UserId == userId)
                .SingleOrDefaultAsync(fv => fv.Id == productId);

            if (product == null)
            {
                return false;
            }

            this.favoriteProductsRepository.Delete(product);
            var result = await this.favoriteProductsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<WishlistViewModel> GetAllProductsFromUserWishlistAsync(string userId)
        {
            var productsInWishlist = await this.favoriteProductsRepository
                .AllAsNoTracking()
                .Where(fp => fp.UserId == userId)
                .To<ProductInWishlistViewModel>()
                .ToListAsync();

            var viewModel = new WishlistViewModel
            {
                Products = productsInWishlist,
            };

            return viewModel;
        }

        public async Task<bool> IsTheProductAlreadyAddedInWishlistAsync(int productId, string userId)
        {
            var result = await this.favoriteProductsRepository
                .AllAsNoTracking()
                .Where(fp => fp.UserId == userId)
                .AnyAsync(fp => fp.ProductId == productId);

            return result;
        }
    }
}
