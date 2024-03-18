namespace TizianaTerenzi.Notifications.Services.Data.Notifications
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Identity;
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Notifications.Data.Models;
    using TizianaTerenzi.Notifications.Web.Models.Notifications;
    using Z.EntityFramework.Plus;

    public class NotificationsService : INotificationsService
    {
        private readonly IDeletableEntityRepository<ApplicationUserNotification> notificationsRepository;

        public NotificationsService(
            IDeletableEntityRepository<ApplicationUserNotification> notificationsRepository)
        {
            this.notificationsRepository = notificationsRepository;
        }

        public async Task<string> AddMessageNotificationAsync(string senderUsername, string senderId, string receiverUsername, string message, string groupId)
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

            await this.notificationsRepository.AddAsync(notification);
            await this.notificationsRepository.SaveChangesAsync();

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
                                    .UpdateAsync(n => new ApplicationUserNotification
                                    {
                                        IsDeleted = true,
                                        DeletedOn = DateTime.UtcNow,
                                    });

            return affectedRows == 1;
        }

        public async Task<bool> DeleteAllNotificationsByUserIdAsync(AllUserNotificationsDeletedMessage message)
        {
            var affectedRows = await this.notificationsRepository
                                    .All()
                                    .Where(n => n.SenderId == message.UserId || n.ReceiverUsername == message.Username)
                                    .UpdateAsync(n => new ApplicationUserNotification
                                    {
                                        IsDeleted = true,
                                        DeletedOn = DateTime.UtcNow,
                                    });

            return affectedRows >= 0;
        }
    }
}
