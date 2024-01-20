namespace TizianaTerenzi.Administration.Data.Models
{
    using TizianaTerenzi.Common.Data.Models;

    public class OrderStatistics : BaseDeletableModel<int>
    {
        public OrderStatistics()
        {
            this.Products = new HashSet<OrderProductStatistics>();
        }

        public int OrderId { get; set; }

        public virtual ICollection<OrderProductStatistics> Products { get; set; }
    }
}
