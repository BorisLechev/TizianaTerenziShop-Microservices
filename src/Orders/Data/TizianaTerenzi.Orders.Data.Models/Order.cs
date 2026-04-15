namespace TizianaTerenzi.Orders.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Models;

    [Index(nameof(UserId))]
    public class Order : BaseDeletableModel<int>
    {
        public Order()
        {
            this.Products = new HashSet<OrderProduct>();
        }

        public string UserId { get; set; }

        public string UserFullName { get; set; }

        public string UserEmail { get; set; }

        public string UserPhoneNumber { get; set; }

        public string UserShippingAddress { get; set; }

        public string UserCountry { get; set; }

        public string UserTown { get; set; }

        public string UserPostalCode { get; set; }

        public int StatusId { get; set; }

        public virtual OrderStatus Status { get; set; }

        public int? DiscountCodeId { get; set; }

        public string? CartDiscountCodeName { get; set; }

        public byte? CartDiscountCodeValue { get; set; }

        public virtual ICollection<OrderProduct> Products { get; set; }
    }
}
