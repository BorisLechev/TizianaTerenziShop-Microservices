namespace TizianaTerenzi.Notifications.Web.Messages
{
    using MassTransit;
    using Microsoft.AspNetCore.SignalR;
    using TizianaTerenzi.Common.Messages.Carts;
    using TizianaTerenzi.Notifications.Services.Data.CartNotifications;
    using TizianaTerenzi.Notifications.Web.Hubs;

    public class NotificationsUpdatedWhenProductAddedInTheCartConsumer : IConsumer<NotificationsUpdatedWhenProductAddedInTheCartMessage>
    {
        private readonly IHubContext<NumberOfProductsInTheUsersCartHub> hubContext;
        private readonly ICartNotificationsService cartNotificationsService;

        public NotificationsUpdatedWhenProductAddedInTheCartConsumer(
            IHubContext<NumberOfProductsInTheUsersCartHub> hubContext,
            ICartNotificationsService cartNotificationsService)
        {
            this.hubContext = hubContext;
            this.cartNotificationsService = cartNotificationsService;
        }

        public async Task Consume(ConsumeContext<NotificationsUpdatedWhenProductAddedInTheCartMessage> context)
        {
            var message = context.Message;

            await this.hubContext.Clients.User(message.UserId).SendAsync("NumberOfProductsInTheUsersCart", message.NumberOfProductsInTheUsersCart);
            var result = await this.cartNotificationsService.AddCartNotificationAsync(message.UserId, message.NumberOfProductsInTheUsersCart);

            await Task.CompletedTask;
        }
    }
}
