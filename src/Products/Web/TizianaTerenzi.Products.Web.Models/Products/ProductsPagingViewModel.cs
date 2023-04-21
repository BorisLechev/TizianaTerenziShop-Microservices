namespace TizianaTerenzi.Products.Web.Models.Products
{
    using TizianaTerenzi.Common.Web.WebModels;
    using TizianaTerenzi.Products.Data.Models;

    public class ProductsPagingViewModel : BasePagingViewModel
    {
        // /products/all
        public string Search { get; set; }

        // /products/all
        public ProductSorting Sorting { get; set; }
    }
}
