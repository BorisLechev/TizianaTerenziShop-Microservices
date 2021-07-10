namespace TizianaTerenzi.Web.Hubs
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

        public async Task AddToGroup(string groupName, string receiverUsername, string senderUsername)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
            await this.chatService.AddUserToGroupAsync(groupName, receiverUsername, senderUsername);
        }

        public async Task SendMessage(string senderUsername, string receiverUsername, string message, string groupName)
        {
            string receiverId = await this.chatService.SendMessageToUserAsync(senderUsername, receiverUsername, message, groupName);
            var notificationId = await this.notificationsService.AddMessageNotificationAsync(senderUsername, receiverUsername, message, groupName);

            var notificationsCount = await this.notificationsService.GetUserNotificationsCountAsync(receiverUsername);

            await this.Clients
                .User(receiverId)
                .SendAsync("ReceiveMessage", senderUsername, new HtmlSanitizer().Sanitize(message.Trim()));

            await this.notificationHubContext
                .Clients
                .User(receiverId)
                .SendAsync("ReceiveNotification", notificationsCount, true);

            var notification = await this.notificationsService.GetNotificationByIdAsync(notificationId);
            await this.notificationHubContext
                .Clients
                .User(receiverId)
                .SendAsync("VisualizeNotification", notification);
        }

        public async Task ReceiveMessage(string senderUsername, string message, string groupName)
        {
            var senderId = await this.chatService.ReceiveNewMessageAsync(senderUsername, message, groupName);

            await this.Clients
                .User(senderId)
                .SendAsync("SendMessage", senderUsername, message.Trim());
        }

        public async Task UserType(string senderUsername, string receiverUsername)
        {
            var receiverId = await this.chatService.UserTypeAsync(senderUsername, receiverUsername);

            await this.Clients
                .User(receiverId)
                .SendAsync("VisualizeUserType", senderUsername, receiverUsername);
        }

        public async Task UserStopType(string receiverUsername)
        {
            var receiverId = await this.chatService.UserStopTypeAsync(receiverUsername);

            await this.Clients
                .User(receiverId)
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
