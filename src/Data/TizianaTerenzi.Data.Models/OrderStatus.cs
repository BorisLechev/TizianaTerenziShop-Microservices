namespace TizianaTerenzi.Data.Models
{
    using TizianaTerenzi.Data.Common.Models;

    public class OrderStatus : BaseDeletableModel<int>
    {
        public string Name { get; set; }
    }
}
