namespace TizianaTerenzi.WebClient.ViewModels.Contacts
{
    using System.ComponentModel.DataAnnotations;

    using TizianaTerenzi.Common.Web.ValidationAttributes;

    public class ContactMessageInputModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, enter your name.")]
        [StringLength(30, ErrorMessage = "Name should be between {2} and {1} characters long.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, enter your email.")]
        [EmailAddress(ErrorMessage = "Please, enter a valid email")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, enter the subject of the email.")]
        [StringLength(20, ErrorMessage = "Subject should be between {2} and {1} characters long.", MinimumLength = 4)]
        public string Subject { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, enter the content of the message.")]
        [StringLength(10000, ErrorMessage = "The content should be between {2} and {1} characters long.", MinimumLength = 5)]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}
