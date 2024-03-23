namespace TizianaTerenzi.WebClient.ViewModels.DiscountCodes
{
    using System;

    public class DiscountCodesListingViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte Discount { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ExpiresOn { get; set; }
    }
}
