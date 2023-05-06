namespace TizianaTerenzi.Web.ViewModels.Subscribe
{
    using System.ComponentModel.DataAnnotations;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class SubscribeInputModel : IMapFrom<Subscriber>
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required.")]
        [StringLength(30, ErrorMessage = "{0} should be between {2} and {1} characters long.", MinimumLength = 5)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
