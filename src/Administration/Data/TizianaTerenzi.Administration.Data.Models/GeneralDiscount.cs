namespace TizianaTerenzi.Administration.Data.Models
{
    using TizianaTerenzi.Common.Data.Models;

    public class GeneralDiscount : BaseModel<int>
    {
        public byte Percent { get; set; }

        public GeneralDiscountCondition IsActive { get; set; }
    }
}
