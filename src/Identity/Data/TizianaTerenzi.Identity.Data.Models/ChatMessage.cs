namespace TizianaTerenzi.Identity.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Data.Models;

    public class ChatMessage : BaseDeletableModel<string>
    {
        public ChatMessage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(GlobalConstants.ChatMessageMaxLength)]
        public string Content { get; set; }

        [Required]
        public string ChatGroupId { get; set; }

        public virtual ChatGroup ChatGroup { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }
    }
}
