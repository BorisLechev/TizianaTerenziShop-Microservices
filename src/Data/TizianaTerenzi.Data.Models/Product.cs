namespace TizianaTerenzi.Data.Models
{
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Models;

    public class Product : BaseDeletableModel<int>
    {
        public Product()
        {
            this.Comments = new HashSet<Comment>();
            this.Notes = new HashSet<ProductNote>();
            this.Votes = new HashSet<ProductVote>();
            this.Carts = new HashSet<Cart>();
            this.ProductInFavoriteCollections = new HashSet<FavoriteProduct>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal PriceWithGeneralDiscount { get; set; }

        public string Picture { get; set; }

        public int YearOfManufacture { get; set; }

        public string SearchText { get; set; }

        public int ProductTypeId { get; set; }

        public virtual ProductType ProductType { get; set; }

        public int FragranceGroupId { get; set; }

        public virtual FragranceGroup FragranceGroup { get; set; }

        public virtual ICollection<ProductNote> Notes { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<ProductVote> Votes { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }

        public virtual ICollection<FavoriteProduct> ProductInFavoriteCollections { get; set; }
    }
}
