namespace TizianaTerenzi.Data.Models
{
    using TizianaTerenzi.Data.Common.Models;

    public class ProductInTheCart : BaseDeletableModel<string> // TODO: make it int
    {
        public int Quantity { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public decimal ProductPriceAfterDiscount { get; set; } // zaradi discount

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int? DiscountCodeId { get; set; }

        public DiscountCode DiscountCode { get; set; }
    }
}
