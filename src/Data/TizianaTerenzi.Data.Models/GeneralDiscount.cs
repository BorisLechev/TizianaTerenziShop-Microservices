namespace TizianaTerenzi.Data.Models
{
    using TizianaTerenzi.Data.Common.Models;

    public class GeneralDiscount : BaseModel<int>
    {
        public int Percent { get; set; }

        public GeneralDiscountCondition IsActive { get; set; }
    }
}
