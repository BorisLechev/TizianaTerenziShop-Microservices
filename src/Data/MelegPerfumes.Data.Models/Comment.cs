namespace MelegPerfumes.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MelegPerfumes.Data.Common.Models;

    public class Comment : BaseDeletableModel<int>
    {
        public Comment()
        {
            this.Votes = new HashSet<Vote>();
        }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int? ParentId { get; set; }

        public virtual Comment Parent { get; set; } // rekursivno sochi kam sebe si diagrama ssms

        public string Content { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }
    }
}
