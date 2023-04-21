namespace TizianaTerenzi.Products.Web.Models.Wishlist
{
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Products.Data.Models;
    using TizianaTerenzi.Products.Services.SlugGenerator;

    public class WishlistViewModel : IMapFrom<FavoriteProduct>
    {
        private readonly ISlugGeneratorService urlGenerator;

        public WishlistViewModel()
            : this(new SlugGeneratorService())
        {
        }

        public WishlistViewModel(ISlugGeneratorService urlGenerator)
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
