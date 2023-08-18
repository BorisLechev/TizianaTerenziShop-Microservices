namespace TizianaTerenzi.Products.Web.Models.Products
{
    using TizianaTerenzi.Common.Enumerators;
    using TizianaTerenzi.Common.Web.WebModels;

    public class ProductsPagingViewModel : BasePagingViewModel
    {
        // /products/all
        public string Search { get; set; }

        // /products/all
        public ProductSorting Sorting { get; set; }
    }
}
