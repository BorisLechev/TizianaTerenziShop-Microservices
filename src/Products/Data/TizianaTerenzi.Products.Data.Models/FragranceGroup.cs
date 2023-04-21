namespace TizianaTerenzi.Products.Data.Models
{
    using TizianaTerenzi.Common.Data.Models;

    public class FragranceGroup : BaseModel<int>
    {
        public FragranceGroup()
        {
            this.Products = new HashSet<Product>();
        }

        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
