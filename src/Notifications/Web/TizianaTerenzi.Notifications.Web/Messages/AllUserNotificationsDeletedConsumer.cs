namespace TizianaTerenzi.Notifications.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Identity;
    using TizianaTerenzi.Notifications.Services.Data.Notifications;

    public class AllUserNotificationsDeletedConsumer : IConsumer<AllUserNotificationsDeletedMessage>
    {
        private readonly INotificationsService notificationsService;

        public AllUserNotificationsDeletedConsumer(INotificationsService notificationsService)
        {
            this.notificationsService = notificationsService;
        }

        public async Task Consume(ConsumeContext<AllUserNotificationsDeletedMessage> context)
        {
            var message = context.Message;

            var result = this.notificationsService.DeleteAllNotificationsByUserIdAsync(message);

            await Task.CompletedTask;
        }
    }
}
