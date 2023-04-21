namespace TizianaTerenzi.Products.Web.Models.Products
{
    using System.Collections.Generic;

    using AutoMapper;
    using Ganss.Xss;
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Products.Data.Models;
    using TizianaTerenzi.Products.Web.Models.Votes;

    public class ProductDetailsViewModel : ProductVoteResponseModel, IMapFrom<Product>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public decimal Price { get; set; }

        public decimal PriceWithGeneralDiscount { get; set; }

        public string Picture { get; set; }

        public IEnumerable<string> Notes { get; set; }

        public string FragranceGroupName { get; set; }

        public int YearOfManufacture { get; set; }

        public double PercentFillStars => this.AverageVote * 20;

        public IEnumerable<ProductCommentViewModel> Comments { get; set; }

        public IEnumerable<RelatedProductsViewModel> RelatedProducts { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductDetailsViewModel>()
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes.Select(n => n.Note.Name)));
        }
    }
}
