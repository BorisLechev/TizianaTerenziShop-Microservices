namespace TizianaTerenzi.WebClient.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class OrdersListingViewModel : OrdersChartResponseModel
    {
        public int Id { get; set; }

        public string UserFullName { get; set; }

        public string DiscountCodeName { get; set; }

        public byte? DiscountCodeDiscount { get; set; }

        public DateTime CreatedOn { get; set; }

        public decimal TotalPrice => this.Products.Sum(p => p.Price * p.Quantity);

        public string StatusName { get; set; }

        public virtual ICollection<OrderProductsListingViewModel> Products { get; set; }
    }
}
