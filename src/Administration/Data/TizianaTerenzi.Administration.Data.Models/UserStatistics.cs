namespace TizianaTerenzi.Administration.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Models;

    [Index(nameof(RoleName))]
    public class UserStatistics : BaseDeletableModel<int>
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string RoleName { get; set; }

        public bool IsBlocked { get; set; }
    }
}
