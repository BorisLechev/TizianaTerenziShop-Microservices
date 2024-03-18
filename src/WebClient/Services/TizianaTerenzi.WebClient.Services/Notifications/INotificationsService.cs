namespace TizianaTerenzi.WebClient.Services.Notifications
{
    using Refit;
    using TizianaTerenzi.WebClient.ViewModels.Notifications;

    public interface INotificationsService
    {
        [Get("/Notifications/UserNotifications")]
        Task<IEnumerable<NotificationViewModel>> GetUserNotificationsAsync();

        [Delete("/Notifications/DeleteNotification")]
        Task<bool> DeleteNotificationAsync(string id);
    }
}
