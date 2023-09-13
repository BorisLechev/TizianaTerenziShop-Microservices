namespace TizianaTerenzi.Carts.Data.Models
{
    using TizianaTerenzi.Common.Data.Models;

    public class DiscountCode : BaseDeletableModel<int>
    {
        //public DiscountCode()
        //{
        //    this.Orders = new HashSet<Order>();
        //}

        public string Name { get; set; }

        public byte Discount { get; set; }

        public DateTime? ExpiresOn { get; set; }

        //public virtual ICollection<Order> Orders { get; set; }
    }
}
