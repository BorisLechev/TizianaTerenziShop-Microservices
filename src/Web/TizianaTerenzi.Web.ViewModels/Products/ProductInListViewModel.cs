namespace TizianaTerenzi.Web.ViewModels.Products
{
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services;
    using TizianaTerenzi.Services.Mapping;

    public class ProductInListViewModel : IMapFrom<Product>
    {
        private readonly ISlugGenerator urlGenerator;

        public ProductInListViewModel()
            : this(new SlugGenerator())
        {
        }

        public ProductInListViewModel(ISlugGenerator urlGenerator)
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
