namespace TizianaTerenzi.Identity.Data.Models
{
    using TizianaTerenzi.Common.Data.Models;

    public class ChatGroup : BaseDeletableModel<string>
    {
        public ChatGroup()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ChatMessages = new HashSet<ChatMessage>();
            this.ChatUserGroups = new HashSet<ChatUserGroup>();
        }

        public virtual ICollection<ChatMessage> ChatMessages { get; set; }

        public virtual ICollection<ChatUserGroup> ChatUserGroups { get; set; }
    }
}
