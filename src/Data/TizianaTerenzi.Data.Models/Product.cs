namespace TizianaTerenzi.Data.Models
{
    using System.Collections.Generic;

    using TizianaTerenzi.Data.Common.Models;

    public class Product : BaseDeletableModel<int>
    {
        public Product()
        {
            this.Comments = new HashSet<Comment>();
            this.Notes = new HashSet<ProductNotes>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal PriceWithDiscount { get; set; }

        public string Picture { get; set; }

        public int ProductTypeId { get; set; }

        public virtual ProductType ProductType { get; set; }

        public int FragranceGroupId { get; set; }

        public virtual FragranceGroup FragranceGroup { get; set; }

        public int YearOfManufacture { get; set; }

        public string SearchText { get; set; }

        public virtual ICollection<ProductNotes> Notes { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
