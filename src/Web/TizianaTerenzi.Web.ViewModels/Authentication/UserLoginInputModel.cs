namespace TizianaTerenzi.Web.ViewModels.Authentication
{
    using System.ComponentModel.DataAnnotations;

    using TizianaTerenzi.Web.Infrastructure.ValidationAttributes;

    public class UserLoginInputModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email or Username field is required.")]
        [Display(Name = "Email or Username")]
        public string EmailOrUserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password field is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}
