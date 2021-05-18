namespace TizianaTerenzi.Services.Data.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.Web.ViewModels.Notifications;
    using Z.EntityFramework.Plus;

    public class NotificationsService : INotificationsService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        private readonly IRepository<NotificationType> notificationTypesRepository;

        private readonly IRepository<ApplicationUserNotification> notificationsRepository;

        public NotificationsService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IRepository<NotificationType> notificationTypesRepository,
            IDeletableEntityRepository<ApplicationUserNotification> notificationsRepository)
        {
            this.usersRepository = usersRepository;
            this.notificationTypesRepository = notificationTypesRepository;
            this.notificationsRepository = notificationsRepository;
        }

        public async Task<string> AddMessageNotificationAsync(string senderUsername, string receiverUsername, string message, string groupName)
        {
            var messageTypeNotification = await this.notificationTypesRepository
                .All()
                .SingleOrDefaultAsync(n => n.Name == "Message");

            var senderId = await this.usersRepository
                .AllAsNoTracking()
                .Where(u => u.UserName == senderUsername)
                .Select(u => u.Id)
                .SingleOrDefaultAsync();

            var notification = new ApplicationUserNotification
            {
                ReceiverUsername = receiverUsername,
                Type = messageTypeNotification,
                Text = message,
                SenderId = senderId,
                Link = $"/chat/with/{senderUsername}/group/{groupName}",
            };

            await this.notificationsRepository.AddAsync(notification);
            await this.notificationsRepository.SaveChangesAsync();

            return notification.Id;
        }

        public async Task<IEnumerable<NotificationViewModel>> GetUserNotificationsAsync(string currentUsername)
        {
            var userNotifications = await this.notificationsRepository
                .AllAsNoTracking()
                .Where(n => n.ReceiverUsername == currentUsername)
                .To<NotificationViewModel>()
                .ToListAsync();

            return userNotifications;
        }

        public async Task<NotificationViewModel> GetNotificationByIdAsync(string id)
        {
            var notification = await this.notificationsRepository
                .AllAsNoTracking()
                .Where(n => n.Id == id)
                .To<NotificationViewModel>()
                .SingleOrDefaultAsync();

            return notification;
        }

        public async Task<int> GetUserNotificationsCountAsync(string username)
        {
            var count = await this.notificationsRepository
                .AllAsNoTracking()
                .CountAsync(n => n.ReceiverUsername == username);

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

        public async Task<bool> DeleteAllNotificationsByUserIdAsync(string userId)
        {
            var affectedRows = await this.notificationsRepository
                .All()
                .Where(n => n.SenderId == userId)
                .UpdateAsync(n => new ApplicationUserNotification
                {
                    IsDeleted = true,
                    DeletedOn = DateTime.UtcNow,
                });

            return affectedRows > 0;
        }
    }
}
