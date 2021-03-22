namespace TizianaTerenzi.Data.Models
{
    using System.Collections.Generic;

    using TizianaTerenzi.Data.Common.Models;

    public class Note : BaseModel<int>
    {
        public Note()
        {
            this.Products = new HashSet<ProductNote>();
        }

        public string Name { get; set; }

        public virtual ICollection<ProductNote> Products { get; set; }
    }
}
