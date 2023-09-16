namespace TizianaTerenzi.Common.Messages.Products
{
    public class ProductAddedInTheCartMessage
    {
        public string UserId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductPicture { get; set; }

        public decimal PriceWithGeneralDiscount { get; set; }
    }
}
