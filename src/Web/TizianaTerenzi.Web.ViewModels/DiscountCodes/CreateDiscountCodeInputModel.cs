namespace TizianaTerenzi.Web.ViewModels.DiscountCodes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreateDiscountCodeInputModel
    {
        private const int NameMinimumLength = 3;
        private const int NameMaximumLength = 30;

        private const int MinimumDiscount = 1;
        private const int MaximumDiscount = 100;

        private const string NameErrorMessage = "Discount code should be at least 3 characters long and not more than 30.";
        private const string DiscountErrorMessage = "Discount should be at least 1% and no more than 100%.";

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(NameMaximumLength, MinimumLength = NameMinimumLength, ErrorMessage = NameErrorMessage)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Discount is required.")]
        [Range(MinimumDiscount, MaximumDiscount, ErrorMessage = DiscountErrorMessage)]
        public byte Discount { get; set; }

        public DateTime? ExpiresOn { get; set; }
    }
}
