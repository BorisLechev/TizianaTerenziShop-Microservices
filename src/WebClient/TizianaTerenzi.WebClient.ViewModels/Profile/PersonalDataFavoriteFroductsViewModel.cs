namespace TizianaTerenzi.WebClient.ViewModels.Profile
{
    using System;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class PersonalDataFavoriteFroductsViewModel : IMapFrom<FavoriteProduct>
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
