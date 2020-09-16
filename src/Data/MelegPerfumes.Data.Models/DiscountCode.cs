namespace MelegPerfumes.Data.Models
{
    using System;

    using MelegPerfumes.Data.Common.Models;

    public class DiscountCode : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public double Discount { get; set; }

        public DateTime? ExpiresOn { get; set; }
    }
}
