namespace TizianaTerenzi.Identity.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using TizianaTerenzi.Common.Data.Models;

    public class ChatUserGroup : BaseDeletableModel<int>
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public string ChatGroupId { get; set; }

        public virtual ChatGroup ChatGroup { get; set; }
    }
}
