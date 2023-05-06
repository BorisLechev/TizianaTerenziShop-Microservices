namespace TizianaTerenzi.WebClient.ViewModels.Profile
{
    using System;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class PersonalDataProductVotesViewModel : IMapFrom<ProductVote>
    {
        public DateTime CreatedOn { get; set; }

        public string ProductName { get; set; }

        public byte Value { get; set; }
    }
}
