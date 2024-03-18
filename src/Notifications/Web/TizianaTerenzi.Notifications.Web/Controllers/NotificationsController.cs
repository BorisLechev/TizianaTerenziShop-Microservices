namespace TizianaTerenzi.Notifications.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Notifications.Services.Data.Notifications;
    using TizianaTerenzi.Notifications.Web.Hubs;
    using TizianaTerenzi.Notifications.Web.Models.Notifications;

    [Authorize]
    public class NotificationsController : ApiController
    {
        private readonly INotificationsService notificationsService;

        private readonly IHubContext<NotificationHub> hubContext;

        public NotificationsController(
            INotificationsService notificationsService,
            IHubContext<NotificationHub> hubContext)
        {
            this.notificationsService = notificationsService;
            this.hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationViewModel>>> UserNotifications()
        {
            var username = this.User.GetUserName();

            var viewModel = await this.notificationsService.GetUserNotificationsAsync(username);

            return this.Ok(viewModel);
        }

        [HttpDelete]
        public async Task<bool> DeleteNotification(string id)
        {
            var userId = this.User.GetUserId();
            var username = this.User.GetUserName();

            var isDeleted = await this.notificationsService.DeleteNotificationAsync(id);
            await this.ChangeNotificationCounterAsync(isDeleted, username, userId);

            return isDeleted;
        }

        private async Task ChangeNotificationCounterAsync(bool isForChange, string username, string userId)
        {
            if (isForChange)
            {
                int count = await this.notificationsService.GetUserNotificationsCountAsync(username);

                await this.hubContext.Clients.User(userId).SendAsync("ReceiveNotificationCount", count, false);
            }
        }
    }
}
