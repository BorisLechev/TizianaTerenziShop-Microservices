namespace TizianaTerenzi.Products.Data.Models
{
    using TizianaTerenzi.Common.Data.Models;

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
