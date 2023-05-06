namespace TizianaTerenzi.WebClient.Hubs
{
    using System.Linq;
    using System.Threading.Tasks;

    using Ganss.XSS;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Chat;
    using TizianaTerenzi.Services.Data.Notifications;
    using TizianaTerenzi.WebClient.ViewModels.Chat;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService chatService;

        private readonly INotificationsService notificationsService;

        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        private readonly IHubContext<NotificationHub> notificationHubContext;

        public ChatHub(
            IChatService chatService,
            INotificationsService notificationsService,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IHubContext<NotificationHub> notificationHubContext)
        {
            this.chatService = chatService;
            this.notificationsService = notificationsService;
            this.usersRepository = usersRepository;
            this.notificationHubContext = notificationHubContext;
        }

        public async Task AddToGroup(string groupId, string receiverUsername, string senderUsername)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupId);
        }

        public async Task SendMessage(string senderUsername, string receiverUsername, string message, string groupId)
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
            string receiverId = await this.chatService.SendMessageToUserAsync(senderUsername, receiverUsername, sanitizedMessage, groupId);

            await this.notificationHubContext
                .Clients
                .User(receiverId)
                .SendAsync("ReceiveNotification", notificationsCount, true);

            var notificationId = await this.notificationsService.AddMessageNotificationAsync(senderUsername, receiverUsername, message, groupId);
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

        public async Task GetUserNotificationsCount(string senderUsername)
        {
            var sender = await this.usersRepository
                .AllAsNoTracking()
                .Where(u => u.UserName == senderUsername)
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                })
                .SingleOrDefaultAsync();

            if (sender != null)
            {
                var count = await this.notificationsService.GetUserNotificationsCountAsync(sender.UserName);

                await this.notificationHubContext
                    .Clients
                    .User(sender.Id)
                    .SendAsync("ReceiveNotification", count, false);
            }
        }
    }
}
