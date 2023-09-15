namespace TizianaTerenzi.Carts.Data.Models
{
    using TizianaTerenzi.Common.Data.Models;

    public class Cart : BaseDeletableModel<string> // TODO: make it int
    {
        public Cart()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public decimal PriceWithDiscountCode { get; set; }

        public int Quantity { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductPicture { get; set; }

        public decimal ProductPriceWithGeneralDiscount { get; set; }

        public string UserId { get; set; }

        public int? DiscountCodeId { get; set; }

        public DiscountCode DiscountCode { get; set; }
    }
}
