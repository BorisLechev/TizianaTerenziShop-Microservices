namespace TizianaTerenzi.Web.Areas.Administration.Models.Products
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CreateProductInputModel : IMapTo<Product>
    {
        private const int NameMinimumLength = 2;
        private const int NameMaximumLength = 25;
        private const int DescriptionMinimumLength = 10;
        private const int DescriptionMaximumLength = 1500;
        private const double MinimumPrice = 10;
        private const int MinimumYear = 1700;
        private const int MaximumYear = 2050;

        private const string NameErrorMessage = "Name should be at least 2 characters long and not more than 25.";
        private const string DescriptionErrorMessage = "Description should be at least 10 characters long and not more than 1500.";
        private const string PriceErrorMessage = "Price should be at least €10.";
        private const string YearErrorMessage = "Year should be be after 1700.";

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(NameMaximumLength, MinimumLength = NameMinimumLength, ErrorMessage = NameErrorMessage)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(DescriptionMaximumLength, MinimumLength = DescriptionMinimumLength, ErrorMessage = DescriptionErrorMessage)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(MinimumPrice, double.MaxValue, ErrorMessage = PriceErrorMessage)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Picture is required.")]
        public IFormFile Picture { get; set; }

        [Required]
        public IEnumerable<string> NoteIds { get; set; }

        public IEnumerable<SelectListItem> Notes { get; set; }

        [Required]
        public int ProductTypeId { get; set; }

        public IEnumerable<SelectListItem> ProductTypes { get; set; }

        [Required]
        public int FragranceGroupId { get; set; }

        public IEnumerable<SelectListItem> FragranceGroups { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        [Range(MinimumYear, MaximumYear, ErrorMessage = YearErrorMessage)]
        public int YearOfManufacture { get; set; }
    }
}
