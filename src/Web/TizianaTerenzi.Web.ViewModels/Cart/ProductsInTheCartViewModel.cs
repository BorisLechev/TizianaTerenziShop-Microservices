namespace TizianaTerenzi.Web.ViewModels.Orders
{
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class ProductsInTheCartViewModel : IMapFrom<ProductInTheCart>, IMapTo<ProductInTheCart>
    {
        public string Id { get; set; } // TODO: make it int

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductPicture { get; set; }

        public decimal ProductPriceWithDiscount { get; set; }

        public decimal ProductPriceAfterDiscount { get; set; }

        public decimal TotalPrice => this.ProductPriceAfterDiscount * this.Quantity;

        public int Quantity { get; set; }

        public int? DiscountCodeId { get; set; }

        public string DiscountCodeName { get; set; }
    }
}
