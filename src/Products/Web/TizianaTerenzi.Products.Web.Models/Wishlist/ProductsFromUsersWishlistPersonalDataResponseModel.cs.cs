namespace TizianaTerenzi.Products.Web.Models.Wishlist
{
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Products.Data.Models;

    public class ProductsFromUsersWishlistPersonalDataResponseModel : IMapFrom<FavoriteProduct>
    {
        public string ProductName { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
