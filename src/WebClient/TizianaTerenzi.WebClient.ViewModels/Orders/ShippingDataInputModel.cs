namespace TizianaTerenzi.WebClient.ViewModels.Orders
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ShippingDataInputModel
    {
        public string Email { get; set; }

        [Display(Name = "First name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required.")]
        [StringLength(30, ErrorMessage = "{0} should be between {2} and {1} characters long.", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required.")]
        [StringLength(30, ErrorMessage = "{0} should be between {2} and {1} characters long.", MinimumLength = 2)]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Town is required.")]
        [StringLength(50, ErrorMessage = "{0} should be between {2} and {1} characters long.", MinimumLength = 2)]
        public string Town { get; set; }

        [Display(Name = "Country")]
        public int? CountryId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        public string? Country { get; set; }

        [Display(Name = "Postal code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Postal code is required.")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [Display(Name = "Phone number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Phone number is required.")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Display(Name = "Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Address is required.")]
        [StringLength(100, ErrorMessage = "{0} should be between {2} and {1} characters long.", MinimumLength = 2)]
        public string Address { get; set; }
    }
}
