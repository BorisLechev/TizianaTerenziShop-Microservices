namespace TizianaTerenzi.Web.ViewModels.Profile
{
    using System.ComponentModel.DataAnnotations;

    public class ProfileViewModel
    {
        [Display(Name = "Full name")]
        public string FullName { get; set; }

        public string Email { get; set; }
    }
}
