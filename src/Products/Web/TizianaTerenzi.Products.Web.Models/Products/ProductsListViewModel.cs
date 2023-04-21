namespace TizianaTerenzi.Products.Web.Models.Products
{
    using System.Collections.Generic;

    public class ProductsListViewModel : ProductsPagingViewModel
    {
        public IEnumerable<ProductInListViewModel> Products { get; set; }
    }
}
