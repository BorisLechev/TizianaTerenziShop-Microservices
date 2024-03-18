namespace TizianaTerenzi.WebClient.ViewModels.Notifications
{
    using System;

    public class NotificationViewModel
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string SenderId { get; set; }

        public string SenderUsername { get; set; }

        public string Link { get; set; }

        public string Text { get; set; }
    }
}
