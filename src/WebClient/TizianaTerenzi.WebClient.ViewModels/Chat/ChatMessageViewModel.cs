namespace TizianaTerenzi.WebClient.ViewModels.Chat
{
    using System;

    public class ChatMessageViewModel
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string AuthorUserName { get; set; }
    }
}
