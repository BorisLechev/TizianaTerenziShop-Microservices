namespace TizianaTerenzi.Web.ViewModels.Products
{
    using System.Linq;

    using AutoMapper;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.Services.SlugGenerator;

    public class ProductInListViewModel : IMapFrom<Product>, IHaveCustomMappings
    {
        private readonly ISlugGeneratorService urlGenerator;

        public ProductInListViewModel()
            : this(new SlugGeneratorService())
        {
        }

        public ProductInListViewModel(ISlugGeneratorService urlGenerator)
        {
            this.urlGenerator = urlGenerator;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal PriceWithGeneralDiscount { get; set; }

        public int Discount => this.Price == this.PriceWithGeneralDiscount ? 0 : (int)((this.Price - this.PriceWithGeneralDiscount) / this.Price * 100);

        public string Picture { get; set; }

        public int YearOfManufacture { get; set; }

        public double AverageVote { get; set; }

        public double PercentFillStars => this.AverageVote * 20;

        public string Url => this.urlGenerator.GenerateUrl(this.Id, this.Name);

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductInListViewModel>()
                .ForMember(dest => dest.AverageVote, opt => opt.MapFrom(src => src.Votes.Count() == 0
                ? 0
                : src.Votes.Average(v => v.Value)));
        }
    }
}
