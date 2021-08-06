namespace TizianaTerenzi.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using TizianaTerenzi.Data.Common.Models;

    public class Emoji : BaseDeletableModel<int>
    {
        [Required]
        public string Image { get; set; }
    }
}
