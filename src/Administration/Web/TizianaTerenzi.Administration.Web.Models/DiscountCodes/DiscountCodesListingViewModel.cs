namespace TizianaTerenzi.Administration.Web.Models.DiscountCodes
{
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Common.Services.Mapping;

    public class DiscountCodesListingViewModel : IMapFrom<DiscountCodeStatistics>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte Discount { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ExpiresOn { get; set; }
    }
}
