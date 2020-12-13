namespace TizianaTerenzi.Web.ViewModels.Products
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ProductsListViewModel : PagingViewModel
    {
        public IEnumerable<ProductInListViewModel> Products { get; set; }

        public int ProductSortingId { get; set; }

        public IEnumerable<SelectListItem> ProductSortings { get; set; }
    }
}
