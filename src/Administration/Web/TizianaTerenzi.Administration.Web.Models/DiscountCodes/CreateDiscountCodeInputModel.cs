namespace TizianaTerenzi.Administration.Web.Models.DiscountCodes
{
    public class CreateDiscountCodeInputModel
    {
        public string Name { get; set; }

        public byte Discount { get; set; }

        public DateTime? ExpiresOn { get; set; }
    }
}
