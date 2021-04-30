namespace TizianaTerenzi.Data.Models
{
    using System.Collections.Generic;

    using TizianaTerenzi.Data.Common.Models;

    public class OrderStatus : BaseDeletableModel<int>
    {
        public OrderStatus()
        {
            this.Orders = new HashSet<Order>();
        }

        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
