namespace TizianaTerenzi.Carts.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Models;

    [Index(nameof(ProductId))]
    [Index(nameof(UserId))]
    public class Cart : BaseDeletableModel<string> // TODO: make it int
    {
        public Cart()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductPicture { get; set; }

        public string UserId { get; set; }

        public int? DiscountCodeId { get; set; }

        public DiscountCode DiscountCode { get; set; }
    }
}
