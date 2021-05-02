namespace TizianaTerenzi.Web.ViewModels.Authentication
{
    using System.ComponentModel.DataAnnotations;

    using TizianaTerenzi.Web.Infrastructure.ValidationAttributes;

    public class UserRegisterInputModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required.")]
        [StringLength(15, ErrorMessage = "{0} should be between {2} and {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required.")]
        [StringLength(20, ErrorMessage = "{0} should be between {2} and {1} characters long.", MinimumLength = 2)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required.")]
        [StringLength(20, ErrorMessage = "{0} should be between {2} and {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required.")]
        [StringLength(20, ErrorMessage = "The {0} should be between {2} and {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}
