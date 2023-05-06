namespace TizianaTerenzi.Web.ViewModels.Orders
{
    using System.Collections.Generic;

    public class OrdersChartResponseModel
    {
        public IEnumerable<OrdersListingViewModel> Orders { get; set; }
    }
}
