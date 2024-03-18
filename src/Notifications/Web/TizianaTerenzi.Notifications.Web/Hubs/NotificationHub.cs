namespace TizianaTerenzi.Notifications.Web.Hubs
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using TizianaTerenzi.Notifications.Services.Data.Notifications;

    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly INotificationsService notificationsService;

        public NotificationHub(
            INotificationsService notificationsService)
        {
            this.notificationsService = notificationsService;
        }

        public async Task GetUserNotificationsCount(bool playNotificationSound)
        {
            var receiverUsername = this.Context.User.Identity.Name;

            var count = await this.notificationsService.GetUserNotificationsCountAsync(receiverUsername);

            var receiverId = this.Context.UserIdentifier;

            await this.Clients
                .User(receiverId)
                .SendAsync("ReceiveNotificationCount", count, playNotificationSound);
        }
    }
}
