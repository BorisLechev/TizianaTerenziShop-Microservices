namespace MelegPerfumes.Services.Models
{
    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Mapping;

    public class ProductInTheCartServiceModel : IMapFrom<Product>, IMapTo<Product>
    {
        public string Id { get; set; }

        public int Quantity { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
