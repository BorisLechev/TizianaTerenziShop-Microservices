namespace MelegPerfumes.Web.ViewModels.Subscribe
{
    using System.ComponentModel.DataAnnotations;

    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Mapping;

    public class SubscribeInputModel : IMapFrom<Subscriber>
    {
        [Required]
        [EmailAddress]
        [StringLength(50, MinimumLength = 5)]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
