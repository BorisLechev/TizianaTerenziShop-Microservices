namespace TizianaTerenzi.Data.Models
{
    using System.Collections.Generic;

    using TizianaTerenzi.Data.Common.Models;

    public class Note : BaseDeletableModel<int>
    {
        public Note()
        {
            this.Products = new HashSet<ProductNotes>();
        }

        public string Name { get; set; }

        public virtual ICollection<ProductNotes> Products { get; set; }
    }
}
