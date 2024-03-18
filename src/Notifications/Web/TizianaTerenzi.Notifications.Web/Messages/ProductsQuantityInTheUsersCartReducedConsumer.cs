namespace TizianaTerenzi.Notifications.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Carts;
    using TizianaTerenzi.Notifications.Services.Data.CartNotifications;

    public class ProductsQuantityInTheUsersCartReducedConsumer : IConsumer<ProductsQuantityInTheUsersCartReducedMessage>
    {
        private readonly ICartNotificationsService cartNotificationsService;

        public ProductsQuantityInTheUsersCartReducedConsumer(ICartNotificationsService cartNotificationsService)
        {
            this.cartNotificationsService = cartNotificationsService;
        }

        public async Task Consume(ConsumeContext<ProductsQuantityInTheUsersCartReducedMessage> context)
        {
            var message = context.Message;

            var result = await this.cartNotificationsService.ReduceQuantityAsync(message);

            await Task.CompletedTask;
        }
    }
}
