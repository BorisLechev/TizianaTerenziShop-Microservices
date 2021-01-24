namespace TizianaTerenzi.Web.ViewModels.Products
{
    using System.Collections.Generic;

    using TizianaTerenzi.Web.ViewModels.Wishlist;

    public class WishlistViewModel
    {
        public IEnumerable<ProductInWishlistViewModel> Products { get; set; }
    }
}
