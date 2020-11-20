namespace TizianaTerenzi.Web.ViewModels.Authentication
{
    using System.ComponentModel.DataAnnotations;

    using TizianaTerenzi.Web.Infrastructure;

    public class UserLoginInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}
