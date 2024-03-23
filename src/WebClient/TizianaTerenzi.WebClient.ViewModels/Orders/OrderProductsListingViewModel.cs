namespace TizianaTerenzi.WebClient.ViewModels.Orders
{
    using System;

    public class OrderProductsListingViewModel
    {
        public int OrderId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice => this.Quantity * this.Price;

        public byte? OrderDiscountCodeDiscount { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
