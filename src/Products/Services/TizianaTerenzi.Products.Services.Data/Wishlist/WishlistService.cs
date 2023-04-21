namespace TizianaTerenzi.Products.Services.Data.Wishlist
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Products.Data.Models;
    using TizianaTerenzi.Products.Web.Models.Wishlist;
    using Z.EntityFramework.Plus;

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

        public async Task<bool> DeleteAllProductsInTheWishlistAsync(string userId)
        {
            var affectedRows = await this.favoriteProductsRepository
                .All()
                .Where(fp => fp.UserId == userId)
                .UpdateAsync(fp => new FavoriteProduct { IsDeleted = true, ModifiedOn = DateTime.UtcNow });

            return affectedRows > 0;
        }

        public async Task<bool> DeleteProductFromTheWishlistAsync(int productId, string userId)
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

        public async Task<IEnumerable<WishlistViewModel>> GetAllProductsFromUsersWishlistAsync(string userId)
        {
            var productsInWishlist = await this.favoriteProductsRepository
                .AllAsNoTracking()
                .Where(fp => fp.UserId == userId)
                .To<WishlistViewModel>()
                .ToListAsync();

            return productsInWishlist;
        }

        public async Task<bool> HasTheProductAlreadyAddedToTheWishlistAsync(int productId, string userId)
        {
            var result = await this.favoriteProductsRepository
                .AllAsNoTracking()
                .Where(fp => fp.UserId == userId)
                .AnyAsync(fp => fp.ProductId == productId);

            return result;
        }
    }
}
