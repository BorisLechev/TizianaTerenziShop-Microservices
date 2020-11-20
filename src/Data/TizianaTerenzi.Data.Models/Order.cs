namespace TizianaTerenzi.Data.Models
{
    using System.Collections.Generic;

    using TizianaTerenzi.Data.Common.Models;

    public class Order : BaseDeletableModel<int>
    {
        public Order()
        {
            this.Products = new HashSet<OrderProduct>();
        }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int StatusId { get; set; }

        public virtual OrderStatus Status { get; set; }

        public int? DiscountCodeId { get; set; }

        public virtual DiscountCode DiscountCode { get; set; }

        public virtual ICollection<OrderProduct> Products { get; set; }
    }
}
