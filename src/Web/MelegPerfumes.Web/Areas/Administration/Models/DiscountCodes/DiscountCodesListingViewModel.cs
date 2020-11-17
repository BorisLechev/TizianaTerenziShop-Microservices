namespace MelegPerfumes.Web.Areas.Administration.Models.DiscountCodes
{
    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Mapping;

    using System;

    public class DiscountCodesListingViewModel : IMapFrom<DiscountCode>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Discount { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ExpiresOn { get; set; }
    }
}
