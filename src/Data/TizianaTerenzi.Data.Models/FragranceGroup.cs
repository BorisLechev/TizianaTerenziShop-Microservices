namespace TizianaTerenzi.Data.Models
{
    using System.Collections.Generic;

    using TizianaTerenzi.Data.Common.Models;

    public class FragranceGroup : BaseDeletableModel<int>
    {
        public FragranceGroup()
        {
            this.Products = new HashSet<Product>();
        }

        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
