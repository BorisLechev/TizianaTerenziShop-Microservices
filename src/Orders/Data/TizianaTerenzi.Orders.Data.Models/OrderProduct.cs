namespace TizianaTerenzi.Orders.Data.Models
{
    using TizianaTerenzi.Common.Data.Models;

    public class OrderProduct : BaseDeletableModel<int>
    {
        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}
