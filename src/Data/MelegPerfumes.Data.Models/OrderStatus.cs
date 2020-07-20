namespace MelegPerfumes.Data.Models
{
    using MelegPerfumes.Data.Common.Models;

    public class OrderStatus : BaseDeletableModel<int>
    {
        public string Name { get; set; }
    }
}
