namespace TizianaTerenzi.Web.ViewModels.Wishlist
{
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.Services.SlugGenerator;

    public class ProductInWishlistViewModel : IMapFrom<FavoriteProduct>
    {
        private readonly ISlugGeneratorService urlGenerator;

        public ProductInWishlistViewModel()
            : this(new SlugGeneratorService())
        {
        }

        public ProductInWishlistViewModel(ISlugGeneratorService urlGenerator)
        {
            this.urlGenerator = urlGenerator;
        }

        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public decimal ProductPriceWithGeneralDiscount { get; set; }

        public int Discount => this.ProductPrice == this.ProductPriceWithGeneralDiscount ? 0 : (int)((this.ProductPrice - this.ProductPriceWithGeneralDiscount) / this.ProductPrice * 100);

        public string ProductPicture { get; set; }

        public int ProductYearOfManufacture { get; set; }

        public string Url => this.urlGenerator.GenerateUrl(this.ProductId, this.ProductName);
    }
}
