namespace MelegPerfumes.Services.Models
{
    using System;

    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Mapping;

    public class ProductInTheCartServiceModel : IMapFrom<Product>, IMapTo<Product>
    {
        public string Id { get; set; }

        public DateTime IssuedOn { get; set; }

        public int Quantity { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public string IssuerId { get; set; }

        public ApplicationUser Issuer { get; set; }
    }
}
