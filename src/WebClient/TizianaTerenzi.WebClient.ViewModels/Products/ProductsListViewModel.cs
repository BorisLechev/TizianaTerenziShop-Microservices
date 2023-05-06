namespace TizianaTerenzi.WebClient.ViewModels.Products
{
    using System.Collections.Generic;

    public class ProductsListViewModel : PagingViewModel
    {
        public IEnumerable<ProductInListViewModel> Products { get; set; }
    }
}
