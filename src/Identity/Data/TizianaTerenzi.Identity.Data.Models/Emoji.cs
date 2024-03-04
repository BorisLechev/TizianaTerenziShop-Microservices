namespace TizianaTerenzi.Identity.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using TizianaTerenzi.Common.Data.Models;

    public class Emoji : BaseDeletableModel<int>
    {
        [Required]
        public string Image { get; set; }
    }
}
