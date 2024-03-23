namespace TizianaTerenzi.Products.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using TizianaTerenzi.Common.Data.Models;
    using TizianaTerenzi.Common.Enumerators;

    public class CommentVote : BaseDeletableModel<int>
    {
        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; }

        [Required]
        public string UserId { get; set; }

        //public virtual ApplicationUser User { get; set; }

        public CommentVoteType Type { get; set; }
    }
}
