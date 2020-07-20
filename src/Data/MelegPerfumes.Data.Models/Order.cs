namespace MelegPerfumes.Data.Models
{
    using System;
    using System.Collections.Generic;

    using MelegPerfumes.Data.Common.Models;

    public class Order : BaseDeletableModel<int>
    {
        public Order()
        {
            this.Products = new HashSet<OrderProduct>();
        }

        public string IssuerId { get; set; }

        public virtual ApplicationUser Issuer { get; set; }

        public int StatusId { get; set; }

        public virtual OrderStatus Status { get; set; }

        public DateTime IssuedOn { get; set; }

        public virtual ICollection<OrderProduct> Products { get; set; }
    }
}
