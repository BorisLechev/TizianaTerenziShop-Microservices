namespace TizianaTerenzi.Identity.Web.Models.Profile
{
    using System;

    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Identity.Data.Models;

    public class DownloadPersonalDataViewModel : IMapFrom<ApplicationUser>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public string Town { get; set; }

        public string PostalCode { get; set; }

        public string CountryName { get; set; }

        public string Address { get; set; }

        public ICollection<PersonalDataChatUserGroupViewModel> ChatUserGroups { get; set; }
    }
}
