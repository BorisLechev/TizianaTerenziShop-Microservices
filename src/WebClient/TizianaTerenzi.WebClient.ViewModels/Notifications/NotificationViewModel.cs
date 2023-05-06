namespace TizianaTerenzi.WebClient.ViewModels.Notifications
{
    using System;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

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
