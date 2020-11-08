namespace MelegPerfumes.Web.ViewModels.Products
{
    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services;
    using MelegPerfumes.Services.Mapping;

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

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Picture { get; set; }

        public int YearOfManufacture { get; set; }

        public string Url => this.urlGenerator.GenerateUrl(this.Id, this.Name);
    }
}
