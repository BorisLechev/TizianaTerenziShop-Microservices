namespace TizianaTerenzi.Web.ViewModels.Profile
{
    using System;
    using System.Collections.Generic;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

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

        public ICollection<PersonalDataOrdersViewModel> Orders { get; set; }

        public ICollection<PersonalDataCommentsViewModel> Comments { get; set; }

        public ICollection<PersonalDataFavoriteFroductsViewModel> FavoriteProducts { get; set; }

        public ICollection<PersonalDataProductVotesViewModel> ProductVotes { get; set; }
    }
}
