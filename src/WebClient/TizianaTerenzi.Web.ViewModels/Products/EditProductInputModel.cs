namespace TizianaTerenzi.Web.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.WebClient.Infrastructure.ValidationAttributes;

    public class EditProductInputModel : IMapFrom<Product>, IHaveCustomMappings
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required.")]
        [StringLength(25, ErrorMessage = "Name should be between {2} and {1} characters long.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required.")]
        [StringLength(1500, ErrorMessage = "Description should be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Description { get; set; }

        [Range(10, double.MaxValue, ErrorMessage = "Price should be at least €{1}.")]
        public decimal Price { get; set; }

        [DataType(DataType.Upload)]
        [AllowedExtensions]
        [MaxFileSize]
        public IFormFile Picture { get; set; }

        [Required]
        [Display(Name = "Notes")]
        public IEnumerable<string> NoteIds { get; set; }

        public IEnumerable<SelectListItem> Notes { get; set; }

        [Display(Name = "Product Type")]
        public int ProductTypeId { get; set; }

        public IEnumerable<SelectListItem> ProductTypes { get; set; }

        [Display(Name = "Fragrance Group")]
        public int FragranceGroupId { get; set; }

        public IEnumerable<SelectListItem> FragranceGroups { get; set; }

        [ProductYearMinMaxValue(2000)]
        public int YearOfManufacture { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, EditProductInputModel>()
                .ForMember(dest => dest.Notes, opt => opt.Ignore())
                .ForMember(dest => dest.Picture, opt => opt.Ignore());
        }
    }
}
