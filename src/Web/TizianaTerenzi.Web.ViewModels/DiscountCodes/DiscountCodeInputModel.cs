namespace TizianaTerenzi.Web.ViewModels.DiscountCodes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DiscountCodeInputModel
    {
        private const int NameMinimumLength = 3;
        private const int NameMaximumLength = 30;

        private const string NameErrorMessage = "Discount code should be at least 3 characters long and not more than 30.";

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(NameMaximumLength, MinimumLength = NameMinimumLength, ErrorMessage = NameErrorMessage)]
        public string Name { get; set; }
    }
}
