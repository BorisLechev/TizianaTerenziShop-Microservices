namespace TizianaTerenzi.Common.Messages.Carts
{
    public class ProductsInTheUserCartHaveBeenOrderedMessage
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string ShippingAddress { get; set; }

        public string Country { get; set; }

        public string Town { get; set; }

        public string PostalCode { get; set; }

        public IEnumerable<ProductsInTheCartMessage> Products { get; set; }
    }

    public class ProductsInTheCartMessage
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice => this.Price * this.Quantity;

        public int Quantity { get; set; }

        public int? DiscountCodeId { get; set; }

        public string DiscountCodeName { get; set; }

        public byte? DiscountCodeDiscount { get; set; }
    }
}
