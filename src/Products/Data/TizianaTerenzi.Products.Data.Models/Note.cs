namespace TizianaTerenzi.Products.Data.Models
{
    using TizianaTerenzi.Common.Data.Models;

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
