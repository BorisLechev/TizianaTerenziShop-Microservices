namespace TizianaTerenzi.Web.ViewModels.Products
{
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services;
    using TizianaTerenzi.Services.Mapping;

    public class ProductsListingViewModel : IMapFrom<Product>
    {
        private readonly IUrlGenerator urlGenerator;

        public ProductsListingViewModel()
            : this(new UrlGenerator())
        {
        }

        public ProductsListingViewModel(IUrlGenerator urlGenerator)
        {
            this.urlGenerator = urlGenerator;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Picture { get; set; }

        public string Url => this.urlGenerator.GenerateUrl(this.Id, this.Name);
    }
}
