namespace TizianaTerenzi.WebClient.ViewModels.PDF
{
    using System.Collections.Generic;

    using TizianaTerenzi.WebClient.ViewModels.Orders;

    public class ExportPdfUserOrderProductsViewModel
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public IEnumerable<OrderProductsListingViewModel> Products { get; set; }
    }
}
