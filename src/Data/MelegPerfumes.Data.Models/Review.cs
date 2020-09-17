namespace MelegPerfumes.Data.Models
{
    using MelegPerfumes.Data.Common.Models;

    public class Review : BaseDeletableModel<int>
    {
        public string ReviewText { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public string IssuerId { get; set; }

        public virtual ApplicationUser Issuer { get; set; }
    }
}
