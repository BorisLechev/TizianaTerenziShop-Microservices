namespace TizianaTerenzi.Notifications.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Carts;
    using TizianaTerenzi.Notifications.Services.Data.CartNotifications;

    public class ProductsQuantityInTheUsersCartDeletedConsumer : IConsumer<ProductsQuantityInTheUsersCartDeletedMessage>
    {
        private readonly ICartNotificationsService cartNotificationsService;

        public ProductsQuantityInTheUsersCartDeletedConsumer(ICartNotificationsService cartNotificationsService)
        {
            this.cartNotificationsService = cartNotificationsService;
        }

        public async Task Consume(ConsumeContext<ProductsQuantityInTheUsersCartDeletedMessage> context)
        {
            var message = context.Message;

            var result = await this.cartNotificationsService.DeleteAllProductsInTheCartByUserIdAsync(message);

            await Task.CompletedTask;
        }
    }
}
