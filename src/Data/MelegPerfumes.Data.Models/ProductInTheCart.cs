namespace MelegPerfumes.Data.Models
{
    using System;

    using MelegPerfumes.Data.Common.Models;

    public class ProductInTheCart : BaseDeletableModel<string>
    {
        public DateTime IssuedOn { get; set; }

        public int Quantity { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public string IssuerId { get; set; }

        public ApplicationUser Issuer { get; set; }

        public int? DiscountCodeId { get; set; }
    }
}
