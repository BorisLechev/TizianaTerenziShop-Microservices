namespace TizianaTerenzi.Web.ViewModels.Notifications
{
    using System;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class NotificationViewModel : IMapFrom<ApplicationUserNotification>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Heading => $"<a href=\"/Profile/{this.SenderId}\" class=\"links-in-heading\">{this.SenderUsername}</a> send you a new <a href=\"{this.Link}\" class=\"links-in-heading\">message</a>.";

        public string SenderId { get; set; }

        public string SenderUsername { get; set; }

        public string ReceiverUsername { get; set; }

        public string Link { get; set; }

        public string Text { get; set; }
    }
}
