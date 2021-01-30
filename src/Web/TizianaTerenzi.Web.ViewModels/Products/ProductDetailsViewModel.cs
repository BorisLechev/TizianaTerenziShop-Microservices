namespace TizianaTerenzi.Web.ViewModels.Products
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Ganss.XSS;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class ProductDetailsViewModel : IMapFrom<Product>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public decimal Price { get; set; }

        public decimal PriceWithDiscount { get; set; }

        public string Picture { get; set; }

        public virtual IEnumerable<string> Notes { get; set; }

        public string FragranceGroupName { get; set; }

        public int YearOfManufacture { get; set; }

        public double AverageVote { get; set; }

        public double PercentFillStars => this.AverageVote * 20;

        public int NumberOfVoters { get; set; }

        public IEnumerable<ProductCommentViewModel> Comments { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductDetailsViewModel>()
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes.Select(n => n.Note.Name)))
                .ForMember(dest => dest.AverageVote, opt => opt.MapFrom(src => src.Votes.Count() == 0 ? 0 : src.Votes.Average(v => v.Value)));
        }
    }
}
