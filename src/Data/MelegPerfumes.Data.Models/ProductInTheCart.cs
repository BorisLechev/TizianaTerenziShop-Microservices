namespace MelegPerfumes.Data.Models
{
    using MelegPerfumes.Data.Common.Models;

    public class ProductInTheCart : BaseDeletableModel<string>
    {
        public int Quantity { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int? DiscountCodeId { get; set; }
    }
}
