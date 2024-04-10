namespace TizianaTerenzi.Notifications.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Carts;
    using TizianaTerenzi.Notifications.Services.Data.CartNotifications;

    public class ProductQuantityInTheUsersCartDeletedConsumer : IConsumer<ProductQuantityInTheUsersCartDeletedMessage>
    {
        private readonly ICartNotificationsService cartNotificationsService;

        public ProductQuantityInTheUsersCartDeletedConsumer(ICartNotificationsService cartNotificationsService)
        {
            this.cartNotificationsService = cartNotificationsService;
        }

        public async Task Consume(ConsumeContext<ProductQuantityInTheUsersCartDeletedMessage> context)
        {
            var message = context.Message;

            var result = await this.cartNotificationsService.DeleteProductInTheCartAsync(message);

            await Task.CompletedTask;
        }
    }
}
