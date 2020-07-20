namespace MelegPerfumes.Data.Models
{
    using System.Collections.Generic;

    using MelegPerfumes.Data.Common.Models;

    public class Product : BaseDeletableModel<int>
    {
        public Product()
        {
            this.Reviews = new HashSet<Review>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int ProductTypeId { get; set; }

        public virtual ProductType ProductType { get; set; }

        public int FragranceGroupId { get; set; }

        public virtual FragranceGroup FragranceGroup { get; set; }

        public int YearOfManufacture { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
