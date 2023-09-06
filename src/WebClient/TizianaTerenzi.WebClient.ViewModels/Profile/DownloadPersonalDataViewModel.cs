namespace TizianaTerenzi.WebClient.ViewModels.Profile
{
    using System;
    using System.Collections.Generic;

    public class DownloadPersonalDataViewModel
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

        public ICollection<PersonalDataOrdersViewModel> Orders { get; set; }

        public ICollection<PersonalDataCommentsViewModel> Comments { get; set; }

        public ICollection<PersonalDataFavoriteFroductsViewModel> FavoriteProducts { get; set; }

        public ICollection<PersonalDataProductVotesViewModel> ProductVotes { get; set; }

        public ICollection<PersonalDataChatUserGroupViewModel> ChatUserGroups { get; set; }
    }
}
