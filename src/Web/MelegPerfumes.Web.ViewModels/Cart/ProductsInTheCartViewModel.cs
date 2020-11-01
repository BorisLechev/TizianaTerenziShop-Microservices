namespace MelegPerfumes.Web.ViewModels.Orders
{
    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Mapping;

    public class ProductsInTheCartViewModel : IMapFrom<ProductInTheCart>, IMapTo<ProductInTheCart>
    {
        public string Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductPicture { get; set; }

        public decimal ProductPrice { get; set; }

        public int Quantity { get; set; }
    }
}
