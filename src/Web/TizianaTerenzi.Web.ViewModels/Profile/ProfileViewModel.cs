namespace TizianaTerenzi.Web.ViewModels.Profile
{
    using System.ComponentModel.DataAnnotations;

    public class ProfileViewModel
    {
        [Display(Name = "Full name")]
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Town { get; set; }

        public string PostalCode { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }
    }
}
