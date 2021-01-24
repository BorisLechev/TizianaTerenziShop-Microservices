namespace TizianaTerenzi.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using TizianaTerenzi.Data.Common.Models;

    public class FavoriteProduct : BaseDeletableModel<int>
    {
        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
