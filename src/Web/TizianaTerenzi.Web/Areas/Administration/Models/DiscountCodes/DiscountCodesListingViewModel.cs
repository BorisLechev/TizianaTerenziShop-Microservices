namespace TizianaTerenzi.Web.Areas.Administration.Models.DiscountCodes
{
    using System;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class DiscountCodesListingViewModel : IMapFrom<DiscountCode>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Discount { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ExpiresOn { get; set; }
    }
}
