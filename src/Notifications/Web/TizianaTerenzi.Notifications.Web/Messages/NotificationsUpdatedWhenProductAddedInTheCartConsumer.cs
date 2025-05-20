namespace TizianaTerenzi.Notifications.Web.Messages
{
    using MassTransit;
    using Microsoft.AspNetCore.SignalR;
    using TizianaTerenzi.Common.Data.EventualConsistencyMessages;
    using TizianaTerenzi.Common.Messages.Carts;
    using TizianaTerenzi.Notifications.Services.Data.CartNotifications;
    using TizianaTerenzi.Notifications.Web.Hubs;

    public class NotificationsUpdatedWhenProductAddedInTheCartConsumer : IConsumer<NotificationsUpdatedWhenProductAddedInTheCartMessage>
    {
        private readonly IHubContext<NumberOfProductsInTheUsersCartHub> hubContext;
        private readonly ICartNotificationsService cartNotificationsService;
        private readonly IEventualConsistencyMessagesService eventualConsistencyMessagesService;

        public NotificationsUpdatedWhenProductAddedInTheCartConsumer(
            IHubContext<NumberOfProductsInTheUsersCartHub> hubContext,
            ICartNotificationsService cartNotificationsService,
            IEventualConsistencyMessagesService eventualConsistencyMessagesService)
        {
            this.hubContext = hubContext;
            this.cartNotificationsService = cartNotificationsService;
            this.eventualConsistencyMessagesService = eventualConsistencyMessagesService;
        }

        public async Task Consume(ConsumeContext<NotificationsUpdatedWhenProductAddedInTheCartMessage> context)
        {
            var message = context.Message;

            var isDuplicated = await this.eventualConsistencyMessagesService.IsDuplicated(
                                            message,
                                            nameof(NotificationsUpdatedWhenProductAddedInTheCartMessage.CorrelationId),
                                            message.CorrelationId);

            if (isDuplicated)
            {
                return;
            }

            await this.hubContext.Clients.User(message.UserId).SendAsync("NumberOfProductsInTheUsersCart", message.NumberOfProductsInTheUsersCart);
            var result = await this.cartNotificationsService.AddCartNotificationAsync(message.UserId, message.NumberOfProductsInTheUsersCart);

            await Task.CompletedTask;
        }
    }
}
