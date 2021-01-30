namespace TizianaTerenzi.Web.ViewModels.Products
{
    using System.Linq;

    using AutoMapper;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services;
    using TizianaTerenzi.Services.Mapping;

    public class ProductInListViewModel : IMapFrom<Product>, IHaveCustomMappings
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

        public decimal PriceWithDiscount { get; set; }

        public int Discount => this.Price == this.PriceWithDiscount ? 0 : (int)((this.Price - this.PriceWithDiscount) / this.Price * 100);

        public string Picture { get; set; }

        public int YearOfManufacture { get; set; }

        public double AverageVote { get; set; }

        public double PercentFillStars => this.AverageVote * 20;

        public string Url => this.urlGenerator.GenerateUrl(this.Id, this.Name);

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductInListViewModel>()
                .ForMember(dest => dest.AverageVote, opt => opt.MapFrom(src => src.Votes.Count() == 0 ? 0 : src.Votes.Average(v => v.Value)));
        }
    }
}
