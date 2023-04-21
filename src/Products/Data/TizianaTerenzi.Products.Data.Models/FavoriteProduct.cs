namespace TizianaTerenzi.Products.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using TizianaTerenzi.Common.Data.Models;

    public class FavoriteProduct : BaseDeletableModel<int>
    {
        [Required]
        public string UserId { get; set; }

        //public virtual ApplicationUser User { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
