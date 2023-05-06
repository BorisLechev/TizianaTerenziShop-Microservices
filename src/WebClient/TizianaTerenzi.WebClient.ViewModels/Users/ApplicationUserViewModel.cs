namespace TizianaTerenzi.WebClient.ViewModels.Users
{
    using System;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class ApplicationUserViewModel : IMapFrom<ApplicationUser>
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
