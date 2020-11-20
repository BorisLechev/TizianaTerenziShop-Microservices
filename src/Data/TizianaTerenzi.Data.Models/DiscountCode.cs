namespace TizianaTerenzi.Data.Models
{
    using System;

    using TizianaTerenzi.Data.Common.Models;

    public class DiscountCode : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public double Discount { get; set; }

        public DateTime? ExpiresOn { get; set; }
    }
}
