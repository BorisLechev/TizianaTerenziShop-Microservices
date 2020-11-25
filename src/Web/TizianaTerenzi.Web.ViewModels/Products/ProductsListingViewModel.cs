namespace TizianaTerenzi.Web.ViewModels.Products
{
    using System.Collections.Generic;

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

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Picture { get; set; }

        public int YearOfManufacture { get; set; }

        public string Url => this.urlGenerator.GenerateUrl(this.Id, this.Name);

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public IEnumerable<ProductDetailsViewModel> Products { get; set; }
    }
}
