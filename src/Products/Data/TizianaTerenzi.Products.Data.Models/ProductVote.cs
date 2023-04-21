namespace TizianaTerenzi.Products.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using TizianaTerenzi.Common.Data.Models;

    public class ProductVote : BaseDeletableModel<int>
    {
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        // TODO: Index ???
        public string UserId { get; set; }

        //public virtual ApplicationUser User { get; set; }

        public byte Value { get; set; }
    }
}
