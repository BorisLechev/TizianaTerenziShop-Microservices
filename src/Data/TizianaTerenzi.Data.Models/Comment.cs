namespace TizianaTerenzi.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using TizianaTerenzi.Data.Common.Models;

    public class Comment : BaseDeletableModel<int>
    {
        public Comment()
        {
            this.Votes = new HashSet<CommentVote>();
        }

        public string Content { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int? ParentId { get; set; }

        public virtual Comment Parent { get; set; } // rekursivno sochi kam sebe si diagrama ssms

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<CommentVote> Votes { get; set; }
    }
}
