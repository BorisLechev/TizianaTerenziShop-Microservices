namespace TizianaTerenzi.Carts.Web.Gateway.Models
{
    public class ProductsInTheCartViewModel
    {
        public string Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductPicture { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice { get; set; }

        public int Quantity { get; set; }

        public int? DiscountCodeId { get; set; }

        public string DiscountCodeName { get; set; }

        public byte? DiscountCodeDiscount { get; set; }
    }
}
