namespace TizianaTerenzi.Web.ViewModels.Contacts
{
    using System.ComponentModel.DataAnnotations;

    using TizianaTerenzi.Web.Infrastructure;

    public class ContactMessageInputModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, enter your name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, enter your email")]
        [EmailAddress(ErrorMessage = "Please, enter a valid email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, enter the subject of the email")]
        [StringLength(100, ErrorMessage = "Subject should be at least {2} and no more than {1} characters.", MinimumLength = 4)]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, enter the content of the message")]
        [StringLength(10000, ErrorMessage = "The content should be at least {2} characters.", MinimumLength = 10)]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}
