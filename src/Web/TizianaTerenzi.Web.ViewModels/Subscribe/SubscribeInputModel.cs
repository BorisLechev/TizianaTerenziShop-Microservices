namespace TizianaTerenzi.Web.ViewModels.Subscribe
{
    using System.ComponentModel.DataAnnotations;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class SubscribeInputModel : IMapFrom<Subscriber>
    {
        [Required]
        [EmailAddress]
        [StringLength(50, MinimumLength = 5)]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
