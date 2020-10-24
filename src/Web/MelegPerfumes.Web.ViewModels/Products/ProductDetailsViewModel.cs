namespace MelegPerfumes.Web.ViewModels.Products
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Ganss.XSS;
    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Mapping;

    public class ProductDetailsViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public decimal Price { get; set; }

        public string Picture { get; set; }

        public virtual IEnumerable<string> Notes { get; set; }

        public string ProductType { get; set; }

        public string FragranceGroup { get; set; }

        public int YearOfManufacture { get; set; }

        public IEnumerable<ProductCommentViewModel> Comments { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductDetailsViewModel>()
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes.Select(n => n.Note.Name)));
        }
    }
}
