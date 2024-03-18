namespace TizianaTerenzi.Notifications.Services.Data.CartNotifications
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Carts;
    using TizianaTerenzi.Notifications.Data.Models;
    using Z.EntityFramework.Plus;

    public class CartNotificationsService : ICartNotificationsService
    {
        private readonly IDeletableEntityRepository<ApplicationUserCartNotification> usersCartNotificationsRepository;

        public CartNotificationsService(IDeletableEntityRepository<ApplicationUserCartNotification> usersCartNotificationsRepository)
        {
            this.usersCartNotificationsRepository = usersCartNotificationsRepository;
        }

        public async Task<bool> AddCartNotificationAsync(string userId, int numberOfProductsInTheUsersCart)
        {
            var cartNotification = await this.usersCartNotificationsRepository
                                        .All()
                                        .SingleOrDefaultAsync(x => x.UserId == userId);

            if (cartNotification == null)
            {
                cartNotification = new ApplicationUserCartNotification
                {
                    UserId = userId,
                };

                await this.usersCartNotificationsRepository.AddAsync(cartNotification);
            }

            cartNotification.NumberOfProductsInTheUsersCart = numberOfProductsInTheUsersCart;

            var result = await this.usersCartNotificationsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<int> GetNumberOfProductsInTheUsersCartAsync(string userId)
        {
            var count = await this.usersCartNotificationsRepository
                            .All()
                            .Where(p => p.UserId == userId)
                            .Select(p => p.NumberOfProductsInTheUsersCart)
                            .SumAsync();

            return count;
        }

        public async Task<bool> DeleteAllProductsInTheCartByUserIdAsync(ProductsQuantityInTheUsersCartDeletedMessage message)
        {
            var affectedRows = await this.usersCartNotificationsRepository
                                      .All()
                                      .Where(p => p.UserId == message.UserId)
                                      .UpdateAsync(n => new ApplicationUserCartNotification
                                      {
                                          NumberOfProductsInTheUsersCart = 0,
                                          ModifiedOn = DateTime.Now,
                                      });

            return affectedRows == 1;
        }

        public async Task<bool> IncreaseQuantityAsync(ProductsQuantityInTheUsersCartIncreasedMessage message)
        {
            var productInTheCart = await this.usersCartNotificationsRepository
                                        .All()
                                        .SingleOrDefaultAsync(p => p.UserId == message.UserId);

            if (productInTheCart == null)
            {
                productInTheCart = new ApplicationUserCartNotification
                {
                    UserId = message.UserId,
                    NumberOfProductsInTheUsersCart = 0,
                };

                await this.usersCartNotificationsRepository.AddAsync(productInTheCart);
            }

            productInTheCart.NumberOfProductsInTheUsersCart++;

            int result = await this.usersCartNotificationsRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> ReduceQuantityAsync(ProductsQuantityInTheUsersCartReducedMessage message)
        {
            var productInTheCart = await this.usersCartNotificationsRepository
                                   .All()
                                   .SingleOrDefaultAsync(p => p.UserId == message.UserId);

            productInTheCart.NumberOfProductsInTheUsersCart--;

            int result = await this.usersCartNotificationsRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
