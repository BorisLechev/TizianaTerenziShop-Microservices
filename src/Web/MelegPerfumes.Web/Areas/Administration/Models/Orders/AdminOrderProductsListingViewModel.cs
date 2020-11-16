namespace MelegPerfumes.Web.Areas.Administration.Models.Orders
{
    using System;

    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Mapping;

    public class AdminOrderProductsListingViewModel : IMapFrom<OrderProduct>
    {
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice => this.Quantity * this.Price;

        public DiscountCode DiscountCode { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
