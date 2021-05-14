namespace TizianaTerenzi.Web.ViewModels.Chat
{
    using System;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;

    public class ChatMessageViewModel : IMapFrom<ChatMessage>
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ChatGroupName { get; set; }

        public string AuthorUserName { get; set; }

        public string ReceiverUsername { get; set; }
    }
}
