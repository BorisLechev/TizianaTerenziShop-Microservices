namespace TizianaTerenzi.Common.Messages.Orders
{
    public class OrderAddedInAdminStatisticsMessage
    {
        public int OrderId { get; set; }

        public IEnumerable<OrderedProductsAddedInAdminStatisticsMessage> Products { get; set; }
    }

    public class OrderedProductsAddedInAdminStatisticsMessage
    {
        public string ProductName { get; set; }

        public decimal PriceWithDiscountCode { get; set; }

        public int Quantity { get; set; }
    }
}
