namespace TizianaTerenzi.Identity.Web.Models.Chat
{
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Identity.Data.Models;

    public class ChatMessageViewModel : IMapFrom<ChatMessage>
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string AuthorUserName { get; set; }
    }
}
