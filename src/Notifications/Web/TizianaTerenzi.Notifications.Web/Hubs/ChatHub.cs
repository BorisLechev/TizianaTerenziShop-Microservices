namespace TizianaTerenzi.Notifications.Web.Hubs
{
    using Ganss.Xss;
    using Microsoft.AspNetCore.SignalR;
    using TizianaTerenzi.Notifications.Services.Data.Notifications;
    using TizianaTerenzi.Notifications.Web.Models.Chat;

    public class ChatHub : Hub
    {
        private readonly INotificationsService notificationsService;
        private readonly IHubContext<NotificationHub> notificationHubContext;

        public ChatHub(
            INotificationsService notificationsService,
            IHubContext<NotificationHub> notificationHubContext)
        {
            this.notificationsService = notificationsService;
            this.notificationHubContext = notificationHubContext;
        }

        public async Task AddToGroup(string groupId)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupId);
        }

        public async Task SendMessage(string senderUsername, string senderId, string receiverUsername, string receiverId, string message, string groupId)
        {
            var sanitizedMessage = new HtmlSanitizer().Sanitize(message.Trim());

            var messageViewModel = new ChatMessageViewModel
            {
                Id = groupId,
                AuthorUserName = senderUsername,
                Content = sanitizedMessage,
            };

            await this.Clients
                .Groups(groupId)
                .SendAsync("SendMessage", messageViewModel);

            var notificationsCount = await this.notificationsService.GetUserNotificationsCountAsync(receiverUsername);
            //string receiverId = await this.chatService.SendMessageToUserAsync(senderUsername, receiverUsername, sanitizedMessage, groupId);

            await this.notificationHubContext
                .Clients
                .User(receiverId)
                .SendAsync("ReceiveNotificationCount", notificationsCount + 1, true);

            var notificationId = await this.notificationsService.AddMessageNotificationAsync(senderUsername, senderId, receiverUsername, message, sanitizedMessage, groupId);
            var notification = await this.notificationsService.GetNotificationByIdAsync(notificationId);

            await this.notificationHubContext
                .Clients
                .User(receiverId)
                .SendAsync("VisualizeNotification", notification);
        }

        public async Task UserType(string groupId)
        {
            await this.Clients
                .OthersInGroup(groupId)
                .SendAsync("VisualizeUserType");
        }

        public async Task UserStopType(string groupId)
        {
            await this.Clients
                .OthersInGroup(groupId)
                .SendAsync("VisualizeUserStopType");
        }
    }
}
