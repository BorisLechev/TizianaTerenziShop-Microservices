namespace MelegPerfumes.Data.Models
{
    using MelegPerfumes.Data.Common.Models;

    public class OrderProduct : BaseDeletableModel<int>
    {
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
