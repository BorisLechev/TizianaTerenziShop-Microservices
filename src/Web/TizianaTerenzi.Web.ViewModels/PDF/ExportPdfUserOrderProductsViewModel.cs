namespace TizianaTerenzi.Web.ViewModels.PDF
{
    using System.Collections.Generic;

    using TizianaTerenzi.Web.ViewModels.Orders;

    public class ExportPdfUserOrderProductsViewModel
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public IEnumerable<OrderProductsListingViewModel> Products { get; set; }
    }
}
