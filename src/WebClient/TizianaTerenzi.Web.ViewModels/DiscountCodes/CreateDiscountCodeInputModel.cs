namespace TizianaTerenzi.Web.ViewModels.DiscountCodes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreateDiscountCodeInputModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required.")]
        [StringLength(30, ErrorMessage = "{0} should be between {2} and {1} characters long.", MinimumLength = 3)]
        public string Name { get; set; }

        [Range(1, 100, ErrorMessage = "Discount should be between {1}% and {2}%.")]
        public byte Discount { get; set; }

        public DateTime? ExpiresOn { get; set; }
    }
}
