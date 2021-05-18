namespace TizianaTerenzi.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using TizianaTerenzi.Data.Common.Models;

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

        public int TypeId { get; set; }

        public virtual NotificationType Type { get; set; }

        [Required]
        public string SenderId { get; set; }

        public virtual ApplicationUser Sender { get; set; }
    }
}
