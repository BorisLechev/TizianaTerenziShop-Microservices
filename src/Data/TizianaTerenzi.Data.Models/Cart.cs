namespace TizianaTerenzi.Data.Models
{
    using System;

    using TizianaTerenzi.Data.Common.Models;

    public class Cart : BaseDeletableModel<string> // TODO: make it int
    {
        public Cart()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public int Quantity { get; set; }

        public decimal ProductPriceWithDiscountCode { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int? DiscountCodeId { get; set; }

        public DiscountCode DiscountCode { get; set; }
    }
}
