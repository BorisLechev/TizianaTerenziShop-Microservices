namespace TizianaTerenzi.Orders.Data.Models
{
    using TizianaTerenzi.Common.Data.Models;

    public class Order : BaseDeletableModel<int>
    {
        public Order()
        {
            this.Products = new HashSet<OrderProduct>();
        }

        public string UserId { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public int StatusId { get; set; }

        public virtual OrderStatus Status { get; set; }

        public int? DiscountCodeId { get; set; }

        public virtual ICollection<OrderProduct> Products { get; set; }
    }
}
