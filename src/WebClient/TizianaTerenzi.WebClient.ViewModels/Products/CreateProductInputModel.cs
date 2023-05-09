namespace TizianaTerenzi.WebClient.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using TizianaTerenzi.Common.Web.Infrastructure.ValidationAttributes;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class CreateProductInputModel : IMapTo<Product>
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required.")]
        [StringLength(25, ErrorMessage = "{0} should be between {2} and {1} characters long.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required.")]
        [StringLength(1500, ErrorMessage = "{0} should be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Description { get; set; }

        [Range(10, double.MaxValue, ErrorMessage = "Price should be at least €{1}.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Picture is required.")]
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
    }
}
