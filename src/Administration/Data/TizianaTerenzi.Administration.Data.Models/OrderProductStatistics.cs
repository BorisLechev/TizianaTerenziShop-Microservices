namespace TizianaTerenzi.Administration.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Models;

    [Index(nameof(ProductName))]
    public class OrderProductStatistics : BaseDeletableModel<int>
    {
        [Required]
        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public int OrderStatisticsId { get; set; }

        public virtual OrderStatistics OrderStatistics { get; set; }
    }
}
