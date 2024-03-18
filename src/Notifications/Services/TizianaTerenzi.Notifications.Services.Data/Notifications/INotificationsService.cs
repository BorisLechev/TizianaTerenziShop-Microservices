namespace TizianaTerenzi.Notifications.Services.Data.Notifications
{
    using TizianaTerenzi.Common.Messages.Identity;
    using TizianaTerenzi.Notifications.Web.Models.Notifications;

    public interface INotificationsService
    {
        Task<string> AddMessageNotificationAsync(string senderUsername, string senderId, string receiverUsername, string message, string groupId);

        Task<IEnumerable<NotificationViewModel>> GetUserNotificationsAsync(string currentUsername);

        Task<NotificationViewModel> GetNotificationByIdAsync(string id);

        Task<int> GetUserNotificationsCountAsync(string receiverUsername);

        Task<bool> DeleteNotificationAsync(string id);

        Task<bool> DeleteAllNotificationsByUserIdAsync(AllUserNotificationsDeletedMessage message);
    }
}
