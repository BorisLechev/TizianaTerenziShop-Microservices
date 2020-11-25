namespace TizianaTerenzi.Web.ViewModels.Products
{
    using System.Collections.Generic;

    public class ProductsWithPaginationListingViewModel
    {
        public IEnumerable<ProductsListingViewModel> Products { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int NextPage => this.CurrentPage == this.PagesCount ? this.PagesCount : this.CurrentPage + 1;
    }
}
