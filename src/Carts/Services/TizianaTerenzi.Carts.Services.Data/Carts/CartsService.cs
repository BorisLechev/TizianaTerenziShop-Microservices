namespace TizianaTerenzi.Carts.Services.Data.Carts
{
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Carts.Data.Models;
    using TizianaTerenzi.Carts.Web.Models.Carts;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Administration;
    using TizianaTerenzi.Common.Messages.Carts;
    using TizianaTerenzi.Common.Messages.Products;
    using TizianaTerenzi.Common.Services.Mapping;
    using Z.EntityFramework.Plus;

    public class CartsService : ICartsService
    {
        private readonly IDeletableEntityRepository<Cart> cartsRepository;

        private readonly IBus publisher;

        public CartsService(
            IDeletableEntityRepository<Cart> cartsRepository,
            IBus publisher)
        {
            this.cartsRepository = cartsRepository;
            this.publisher = publisher;
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
                Price = product.Price,
            };

            await this.cartsRepository.AddAsync(productInTheCart);

            int result = await this.cartsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CheckIfProductExistsInTheUsersCartAsync(string userId, int productId)
        {
            var result = await this.cartsRepository
                                .All()
                                .AnyAsync(p => p.UserId == userId && p.ProductId == productId);

            return result;
        }

        public async Task<bool> DeleteAllProductsInTheCartByUserIdAsync(string userId)
        {
            var productsCount = await this.cartsRepository
                                      .All()
                                      .Where(p => p.UserId == userId)
                                      .DeleteAsync();

            if (productsCount > 0)
            {
                await this.publisher.Publish(new ProductsQuantityInTheUsersCartDeletedMessage
                {
                    UserId = userId,
                });
            }

            return productsCount > 0;
        }

        public async Task<bool> DeleteProductInTheCartAsync(string id)
        {
            var productsCount = await this.cartsRepository
                                    .All()
                                    .Where(p => p.Id == id)
                                    .DeleteAsync();

            return productsCount == 1;
        }

        public async Task<bool> DeleteProductInAllCartsAsync(ProductInAllCartsDeletedMessage message)
        {
            var affectedRows = await this.cartsRepository
                                    .All()
                                    .Where(p => p.ProductId == message.ProductId)
                                    .DeleteAsync();

            return affectedRows >= 0;
        }

        public async Task<bool> IsThereAnyProductsInTheUsersCartAsync(string userId)
        {
            var result = await this.cartsRepository
                                .All()
                                .AnyAsync(op => op.UserId == userId);

            return result;
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
                            .All()
                            .Where(p => p.UserId == userId)
                            .Select(p => p.Quantity)
                            .SumAsync();

            return count;
        }

        public async Task<string> GetProductInTheCartIdByProductIdAsync(int productId, string userId)
        {
            var productInTheCartId = await this.cartsRepository
                                            .All()
                                            .Where(p => p.ProductId == productId && p.UserId == userId)
                                            .Select(p => p.Id)
                                            .SingleOrDefaultAsync();

            return productInTheCartId;
        }

        public async Task<bool> IncreaseQuantityAsync(string cartId)
        {
            var productInTheCart = await this.cartsRepository
                                        .All()
                                        .SingleOrDefaultAsync(p => p.Id == cartId);

            productInTheCart.Quantity++;

            int result = await this.cartsRepository.SaveChangesAsync();

            await this.publisher.Publish(new ProductsQuantityInTheUsersCartIncreasedMessage
            {
                UserId = productInTheCart.UserId,
            });

            return result > 0;
        }

        public async Task<bool> ReduceQuantityAsync(string cartId)
        {
            var productInTheCart = await this.cartsRepository
                                   .All()
                                   .SingleOrDefaultAsync(p => p.Id == cartId);

            if (productInTheCart.Quantity <= 1)
            {
                return false;
            }

            productInTheCart.Quantity--;

            int result = await this.cartsRepository.SaveChangesAsync();

            await this.publisher.Publish(new ProductsQuantityInTheUsersCartReducedMessage
            {
                UserId = productInTheCart.UserId,
            });

            return result > 0;
        }

        public async Task Order(ProductsInTheUserCartHaveBeenOrderedInputModel inputModel, string userId)
        {
            var productsInTheCart = await this.GetAllProductsInTheCartByUserIdAsync(userId);

            await this.publisher.PublishBatch(new object[]
            {
                new ProductsInTheUserCartHaveBeenOrderedMessage
                {
                    UserId = userId,
                    Email = inputModel.Email,
                    FullName = inputModel.FullName,
                    Country = inputModel.Country,
                    Town = inputModel.Town,
                    ShippingAddress = inputModel.ShippingAddress,
                    PostalCode = inputModel.PostalCode,
                    PhoneNumber = inputModel.PhoneNumber,
                    Products = productsInTheCart.Select(p => new ProductsInTheCartMessage
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        Price = p.Price,
                        Quantity = p.Quantity,
                        DiscountCodeId = p.DiscountCodeId,
                        DiscountCodeName = p.DiscountCodeName,
                        DiscountCodeDiscount = p.DiscountCodeDiscount,
                    }),
                },
                new UserProfileDataUpdatedAfterProductsInTheCartHaveBeenOrderedMessage
                {
                    UserId = userId,
                    PhoneNumber = inputModel.PhoneNumber,
                    ShippingAddress = inputModel.ShippingAddress,
                    Town = inputModel.Town,
                    Country = inputModel.Country,
                    PostalCode = inputModel.PostalCode,
                },
            });
        }

        public async Task<bool> EditProductInTheCartAsync(ProductInAllCartsEditedMessage message)
        {
            var productsInTheCart = await this.cartsRepository
                                        .All()
                                        .Include(c => c.DiscountCode)
                                        .Where(p => p.ProductId == message.ProductId)
                                        .ToListAsync();

            foreach (var productInTheCart in productsInTheCart)
            {
                productInTheCart.ProductName = message.Name;

                productInTheCart.Price = productInTheCart.DiscountCodeId.HasValue
                                        ? message.Price * (1 - ((decimal)productInTheCart.DiscountCode.Discount / 100))
                                        : message.Price;

                productInTheCart.ModifiedOn = DateTime.UtcNow;
            }

            var affectedRows = await this.cartsRepository.SaveChangesAsync();

            return affectedRows > 0;
        }
    }
}
