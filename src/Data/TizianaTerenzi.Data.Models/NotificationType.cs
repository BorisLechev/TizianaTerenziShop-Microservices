namespace TizianaTerenzi.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using TizianaTerenzi.Data.Common.Models;

    public class NotificationType : BaseModel<int>
    {
        public NotificationType()
        {
            this.UserNotifications = new HashSet<ApplicationUserNotification>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<ApplicationUserNotification> UserNotifications { get; set; }
    }
}
