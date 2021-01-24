namespace TizianaTerenzi.Web.ViewModels.Wishlist
{
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services;
    using TizianaTerenzi.Services.Mapping;

    public class ProductInWishlistViewModel : IMapFrom<FavoriteProduct>
    {
        private readonly ISlugGenerator urlGenerator;

        public ProductInWishlistViewModel()
            : this(new SlugGenerator())
        {
        }

        public ProductInWishlistViewModel(ISlugGenerator urlGenerator)
        {
            this.urlGenerator = urlGenerator;
        }

        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductPicture { get; set; }

        public string Url => this.urlGenerator.GenerateUrl(this.ProductId, this.ProductName);
    }
}
