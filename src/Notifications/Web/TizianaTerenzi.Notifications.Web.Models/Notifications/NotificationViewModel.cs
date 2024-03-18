namespace TizianaTerenzi.Notifications.Web.Models.Notifications
{
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Notifications.Data.Models;

    public class NotificationViewModel : IMapFrom<ApplicationUserNotification>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string SenderId { get; set; }

        public string SenderUsername { get; set; }

        public string Link { get; set; }

        public string Text { get; set; }
    }
}
