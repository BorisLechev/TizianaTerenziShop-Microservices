namespace MelegPerfumes.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using MelegPerfumes.Data.Common.Models;

    public class Vote : BaseDeletableModel<int>
    {
        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public VoteType Type { get; set; }
    }
}
