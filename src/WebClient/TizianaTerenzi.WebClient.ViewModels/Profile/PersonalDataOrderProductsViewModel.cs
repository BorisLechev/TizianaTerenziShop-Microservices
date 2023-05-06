namespace TizianaTerenzi.WebClient.ViewModels.Profile
{
    using System;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class PersonalDataOrderProductsViewModel : IMapFrom<OrderProduct>
    {
        public DateTime CreatedOn { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public int Quantity { get; set; }
    }
}
