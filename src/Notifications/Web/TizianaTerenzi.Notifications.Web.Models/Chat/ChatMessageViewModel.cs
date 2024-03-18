namespace TizianaTerenzi.Notifications.Web.Models.Chat
{
    public class ChatMessageViewModel
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string AuthorUserName { get; set; }
    }
}
