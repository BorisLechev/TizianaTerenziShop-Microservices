namespace MelegPerfumes.Web.ViewModels.Orders
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class OrderInputModel
    {
        [Required]
        public IEnumerable<OrderProductInputModel> OrderProducts { get; set; }
    }
}
