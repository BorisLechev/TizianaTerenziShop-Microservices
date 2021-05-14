namespace TizianaTerenzi.Data.Models
{
    using System;
    using System.Collections.Generic;

    using TizianaTerenzi.Data.Common.Models;

    public class ChatGroup : BaseDeletableModel<string>
    {
        public ChatGroup()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ChatMessages = new HashSet<ChatMessage>();
            this.ChatUserGroups = new HashSet<ChatUserGroup>();
        }

        public string Name { get; set; }

        public virtual ICollection<ChatMessage> ChatMessages { get; set; }

        public virtual ICollection<ChatUserGroup> ChatUserGroups { get; set; }
    }
}
