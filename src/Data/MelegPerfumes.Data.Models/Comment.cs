namespace MelegPerfumes.Data.Models
{
    using System.Collections.Generic;

    using MelegPerfumes.Data.Common.Models;

    public class Comment : BaseDeletableModel<int>
    {

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int? ParentId { get; set; }

        public virtual Comment Parent { get; set; } // rekursivno sochi kam sebe si diagrama ssms

        public string Content { get; set; }

        public string IssuerId { get; set; }

        public virtual ApplicationUser Issuer { get; set; }
    }
}
