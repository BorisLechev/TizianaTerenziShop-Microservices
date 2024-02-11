namespace TizianaTerenzi.Administration.Data.Models
{
    using TizianaTerenzi.Common.Data.Models;

    public class DiscountCodeStatistics : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public byte Discount { get; set; }

        public DateTime? ExpiresOn { get; set; }
    }
}
