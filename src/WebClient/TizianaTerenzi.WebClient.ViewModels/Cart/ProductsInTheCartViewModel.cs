namespace TizianaTerenzi.WebClient.ViewModels.Cart
{
    public class ProductsInTheCartViewModel
    {
        public string Id { get; set; } // TODO: make it int

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductPicture { get; set; }

        public decimal ProductPriceWithGeneralDiscount { get; set; }

        public decimal PriceWithDiscountCode { get; set; }

        public decimal TotalPrice => this.PriceWithDiscountCode * this.Quantity;

        public int Quantity { get; set; }

        public int? DiscountCodeId { get; set; }

        public string DiscountCodeName { get; set; }
    }
}
