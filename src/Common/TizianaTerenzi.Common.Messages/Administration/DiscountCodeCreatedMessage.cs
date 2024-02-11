namespace TizianaTerenzi.Common.Messages.Administration
{
    public class DiscountCodeCreatedMessage
    {
        public string Name { get; set; }

        public byte Discount { get; set; }

        public DateTime? ExpiresOn { get; set; }
    }
}
