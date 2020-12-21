namespace TizianaTerenzi.Data.Models
{
    using System.Collections.Generic;

    using TizianaTerenzi.Data.Common.Models;

    public class ProductType : BaseModel<int>
    {
        public ProductType()
        {
            this.Products = new HashSet<Product>();
        }

        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
