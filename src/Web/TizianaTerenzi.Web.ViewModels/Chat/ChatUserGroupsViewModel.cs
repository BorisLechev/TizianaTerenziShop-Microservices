namespace TizianaTerenzi.Web.ViewModels.Chat
{
    using System.Collections.Generic;

    using TizianaTerenzi.Data.Models;

    public class ChatUserGroupsViewModel
    {
        public string ChatGroupId { get; set; }

        public IEnumerable<ChatUserGroup> ChatUserGroups { get; set; }
    }
}
