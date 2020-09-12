namespace MelegPerfumes.Web.ViewModels.Products
{
    using System.Collections.Generic;

    using AutoMapper;
    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Mapping;

    public class ProductDetailsViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Picture { get; set; }

        public virtual ICollection<ProductNotes> Notes { get; set; }

        public string ProductType { get; set; }

        public string FragranceGroup { get; set; }

        public int YearOfManufacture { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductDetailsViewModel>()
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes));
        }
    }
}
