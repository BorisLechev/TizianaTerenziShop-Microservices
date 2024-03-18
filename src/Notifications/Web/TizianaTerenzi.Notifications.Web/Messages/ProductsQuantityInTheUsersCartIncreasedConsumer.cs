namespace TizianaTerenzi.Notifications.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Carts;
    using TizianaTerenzi.Notifications.Services.Data.CartNotifications;

    public class ProductsQuantityInTheUsersCartIncreasedConsumer : IConsumer<ProductsQuantityInTheUsersCartIncreasedMessage>
    {
        private readonly ICartNotificationsService cartNotificationsService;

        public ProductsQuantityInTheUsersCartIncreasedConsumer(ICartNotificationsService cartNotificationsService)
        {
            this.cartNotificationsService = cartNotificationsService;
        }

        public async Task Consume(ConsumeContext<ProductsQuantityInTheUsersCartIncreasedMessage> context)
        {
            var message = context.Message;

            var result = await this.cartNotificationsService.IncreaseQuantityAsync(message);

            await Task.CompletedTask;
        }
    }
}
