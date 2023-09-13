namespace TizianaTerenzi.Carts.Services.Data.Carts
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Carts.Data.Models;
    using TizianaTerenzi.Carts.Web.Models.Carts;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Services.Mapping;
    using Z.EntityFramework.Plus;

    public class CartsService : ICartsService
    {
        private readonly IDeletableEntityRepository<Cart> cartsRepository;

        public CartsService(IDeletableEntityRepository<Cart> cartsRepository)
        {
            this.cartsRepository = cartsRepository;
        }

        public async Task<IEnumerable<ProductsInTheCartViewModel>> GetAllProductsInTheCartByUserIdAsync(string userId)
        {
            var productsInTheCart = await this.cartsRepository
                                    .All()
                                    .Where(c => c.UserId == userId)
                                    .To<ProductsInTheCartViewModel>()
                                    .ToListAsync();

            return productsInTheCart;
        }

        public async Task<bool> IncreaseQuantityAsync(string productId)
        {
            var productInTheCart = await this.cartsRepository
                                        .All()
                                        .SingleOrDefaultAsync(p => p.Id == productId);

            productInTheCart.Quantity++;

            int result = await this.cartsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> ReduceQuantityAsync(string productId)
        {
            var productInTheCart = await this.cartsRepository
                                   .All()
                                   .SingleOrDefaultAsync(p => p.Id == productId);

            if (productInTheCart.Quantity <= 1)
            {
                return false;
            }

            productInTheCart.Quantity--;

            int result = await this.cartsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteProductInTheCartAsync(string productId)
        {
            var productsCount = await this.cartsRepository
                .AllAsNoTracking()
                .Where(p => p.Id == productId)
                .DeleteAsync();

            return productsCount == 1;
        }
    }
}
