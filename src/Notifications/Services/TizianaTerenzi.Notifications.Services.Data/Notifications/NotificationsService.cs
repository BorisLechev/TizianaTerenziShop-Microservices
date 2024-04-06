namespace TizianaTerenzi.Notifications.Services.Data.Notifications
{
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Data.Models;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Identity;
    using TizianaTerenzi.Common.Messages.Notifications;
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Notifications.Data.Models;
    using TizianaTerenzi.Notifications.Web.Models.Notifications;

    public class NotificationsService : INotificationsService
    {
        private readonly IDeletableEntityRepository<ApplicationUserNotification> notificationsRepository;
        private readonly IBus publisher;

        public NotificationsService(
            IDeletableEntityRepository<ApplicationUserNotification> notificationsRepository,
            IBus publisher)
        {
            this.notificationsRepository = notificationsRepository;
            this.publisher = publisher;
        }

        public async Task<string> AddMessageNotificationAsync(string senderUsername, string senderId, string receiverUsername, string message, string sanitizedMessage, string groupId)
        {
            var notification = new ApplicationUserNotification
            {
                ReceiverUsername = receiverUsername,
                Text = message,
                SenderId = senderId,
                SenderUsername = senderUsername,
                Link = $"/chat/with/{senderUsername}/group/{groupId}",
            };

            // Delete notifications when more than 50
            var notifications = await this.notificationsRepository
                                    .All()
                                    .Where(n => n.ReceiverUsername == receiverUsername)
                                    .OrderBy(n => n.CreatedOn)
                                    .ToListAsync();

            if (notifications.Count + 1 > GlobalConstants.MaxChatNotificationsPerUser)
            {
                notifications = notifications
                                .Take(GlobalConstants.MaxChatNotificationsPerUser - 1)
                                .ToList();

                this.notificationsRepository.DeleteRange(notifications);
            }

            var messageData = new ChatMessageToUserSentMessage
            {
                SendersUsername = senderUsername,
                ReceiversUsername = receiverUsername,
                SanitizedMessage = sanitizedMessage,
                GroupId = groupId,
            };

            var messageLog = new EventMessageLog(messageData);

            await this.notificationsRepository.AddAsync(notification, messageLog);
            await this.notificationsRepository.SaveChangesAsync();

            await this.publisher.Publish(messageData);

            await this.notificationsRepository.MarkEventMessageLogAsPublished(messageLog.Id);

            return notification.Id;
        }

        public async Task<IEnumerable<NotificationViewModel>> GetUserNotificationsAsync(string currentUsername)
        {
            var userNotifications = await this.notificationsRepository
                                        .All()
                                        .Where(n => n.ReceiverUsername == currentUsername)
                                        .To<NotificationViewModel>()
                                        .OrderByDescending(n => n.CreatedOn)
                                        .ToListAsync();

            return userNotifications;
        }

        public async Task<NotificationViewModel> GetNotificationByIdAsync(string id)
        {
            var notification = await this.notificationsRepository
                                    .All()
                                    .Where(n => n.Id == id)
                                    .To<NotificationViewModel>()
                                    .SingleOrDefaultAsync();

            return notification;
        }

        public async Task<int> GetUserNotificationsCountAsync(string receiverUsername)
        {
            var count = await this.notificationsRepository
                            .All()
                            .CountAsync(n => n.ReceiverUsername == receiverUsername);

            return count;
        }

        public async Task<bool> DeleteNotificationAsync(string id)
        {
            var affectedRows = await this.notificationsRepository
                                    .All()
                                    .Where(n => n.Id == id)
                                    .ExecuteUpdateAsync(setters => setters
                                        .SetProperty(n => n.IsDeleted, true)
                                        .SetProperty(n => n.DeletedOn, DateTime.UtcNow));

            return affectedRows == 1;
        }

        public async Task<bool> DeleteAllNotificationsByUserIdAsync(AllUserNotificationsDeletedMessage message)
        {
            var affectedRows = await this.notificationsRepository
                                    .All()
                                    .Where(n => n.SenderId == message.UserId || n.ReceiverUsername == message.Username)
                                    .ExecuteUpdateAsync(setters => setters
                                        .SetProperty(n => n.IsDeleted, true)
                                        .SetProperty(n => n.DeletedOn, DateTime.UtcNow));

            return affectedRows >= 0;
        }
    }
}
