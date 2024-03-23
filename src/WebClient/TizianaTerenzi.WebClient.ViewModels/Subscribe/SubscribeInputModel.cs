namespace TizianaTerenzi.WebClient.ViewModels.Subscribe
{
    using System.ComponentModel.DataAnnotations;

    public class SubscribeInputModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required.")]
        [StringLength(30, ErrorMessage = "{0} should be between {2} and {1} characters long.", MinimumLength = 5)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
