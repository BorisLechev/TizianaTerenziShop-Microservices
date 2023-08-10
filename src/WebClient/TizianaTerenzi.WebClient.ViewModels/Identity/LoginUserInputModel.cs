namespace TizianaTerenzi.WebClient.ViewModels.Identity
{
    using System.ComponentModel.DataAnnotations;

    public class LoginUserInputModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email or Username field is required.")]
        public string EmailOrUserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password field is required.")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
