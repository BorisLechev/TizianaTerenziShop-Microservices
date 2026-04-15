namespace TizianaTerenzi.Notifications.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Models;

    [Index(nameof(UserId), IsUnique = true)]
    public class ApplicationUserCartNotification : BaseDeletableModel<int>
    {
        [Required]
        public string UserId { get; set; }

        public int NumberOfProductsInTheUsersCart { get; set; }
    }
}
