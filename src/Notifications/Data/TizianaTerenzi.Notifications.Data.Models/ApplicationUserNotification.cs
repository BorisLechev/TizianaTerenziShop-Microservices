namespace TizianaTerenzi.Notifications.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Models;

    [Index(nameof(this.ReceiverUsername))]
    [Index(nameof(this.SenderId))]
    public class ApplicationUserNotification : BaseDeletableModel<string>
    {
        public ApplicationUserNotification()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string ReceiverUsername { get; set; }

        [Required]
        public string Link { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Required]
        public string SenderUsername { get; set; }
    }
}
