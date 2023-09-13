namespace TizianaTerenzi.Carts.Web.Models.Carts
{
    using TizianaTerenzi.Carts.Data.Models;
    using TizianaTerenzi.Common.Services.Mapping;

    public class ProductsInTheCartViewModel : IMapFrom<Cart>, IMapTo<Cart>
    {
        public string Id { get; set; } // TODO: make it int

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductPicture { get; set; }

        public decimal ProductPriceWithGeneralDiscount { get; set; }

        public decimal ProductPriceWithDiscountCode { get; set; }

        public decimal TotalPrice => this.ProductPriceWithDiscountCode * this.Quantity;

        public int Quantity { get; set; }

        public int? DiscountCodeId { get; set; }

        public string DiscountCodeName { get; set; }
    }
}
