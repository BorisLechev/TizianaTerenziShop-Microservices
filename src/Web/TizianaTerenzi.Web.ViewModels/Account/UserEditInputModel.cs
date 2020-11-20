using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TizianaTerenzi.Web.ViewModels.Account
{
    public class UserEditInputModel
    {
        public string Email { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
    }
}
