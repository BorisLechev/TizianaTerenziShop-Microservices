namespace TizianaTerenzi.Carts.Services.Data.Carts
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Carts.Data.Models;
    using TizianaTerenzi.Carts.Web.Models.Carts;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Products;
    using TizianaTerenzi.Common.Services.Mapping;
    using Z.EntityFramework.Plus;

    public class CartsService : ICartsService
    {
        private readonly IDeletableEntityRepository<Cart> cartsRepository;

        public CartsService(IDeletableEntityRepository<Cart> cartsRepository)
        {
            this.cartsRepository = cartsRepository;
        }

        public async Task<bool> AddProductInTheCartAsync(ProductAddedInTheCartMessage product)
        {
            var productInTheCart = new Cart
            {
                UserId = product.UserId,
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductPicture = product.ProductPicture,
                Quantity = 1,
                PriceWithDiscountCode = product.PriceWithGeneralDiscount,
                ProductPriceWithGeneralDiscount = product.PriceWithGeneralDiscount,
            };

            await this.cartsRepository.AddAsync(productInTheCart);

            int result = await this.cartsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CheckIfProductExistsInTheUsersCartAsync(string userId, int productId)
        {
            var result = await this.cartsRepository
                                .AllAsNoTracking()
                                .AnyAsync(p => p.UserId == userId && p.ProductId == productId);

            return result;
        }

        public async Task<bool> DeleteAllProductsInTheCartByUserIdAsync(string userId)
        {
            var productsCount = await this.cartsRepository
                                      .AllAsNoTracking()
                                      .Where(p => p.UserId == userId)
                                      .DeleteAsync();

            return productsCount > 0;
        }

        public async Task<bool> DeleteProductInTheCartAsync(string productId)
        {
            var productsCount = await this.cartsRepository
                                    .AllAsNoTracking()
                                    .Where(p => p.Id == productId)
                                    .DeleteAsync();

            return productsCount == 1;
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

        public async Task<int> GetNumberOfProductsInTheUsersCart(string userId)
        {
            var count = await this.cartsRepository
                            .AllAsNoTracking()
                            .Where(p => p.UserId == userId)
                            .Select(p => p.Quantity)
                            .SumAsync();

            return count;
        }

        public async Task<string> GetProductInTheCartIdByProductIdAsync(int productId, string userId)
        {
            var productInTheCartId = await this.cartsRepository
                                            .AllAsNoTracking()
                                            .Where(p => p.ProductId == productId && p.UserId == userId)
                                            .Select(p => p.Id)
                                            .SingleOrDefaultAsync();

            return productInTheCartId;
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
    }
}
