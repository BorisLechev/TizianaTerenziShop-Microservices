namespace TizianaTerenzi.WebClient.ViewModels.Users
{
    using System;

    public class ApplicationUserViewModel
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{this.FirstName} {this.LastName}";

        public string Role { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Town { get; set; }

        public string CountryName { get; set; }

        public string Address { get; set; }
    }
}
