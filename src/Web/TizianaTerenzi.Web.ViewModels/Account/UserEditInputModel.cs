namespace TizianaTerenzi.Web.ViewModels.Account
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.Web.ViewModels.ValidationAttributes;

    public class UserEditInputModel : IMapFrom<ApplicationUser>
    {
        [MinLength(2)]
        [MaxLength(30)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Town { get; set; }

        [Display(Name = "Country")]
        public int? CountryId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        [Display(Name ="Postal code")]
        public string PostalCode { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Profile Picture")]
        [DataType(DataType.Upload)]
        [AllowedExtensions]
        [MaxFileSize]
        public IFormFile AvatarImage { get; set; }
    }
}
