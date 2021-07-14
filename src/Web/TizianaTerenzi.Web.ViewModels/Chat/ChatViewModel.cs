namespace TizianaTerenzi.Web.ViewModels.Chat
{
    using System.Collections.Generic;

    public class ChatViewModel
    {
        public string SenderId { get; set; }

        public string SenderUsername { get; set; }

        public string ReceiverId { get; set; }

        public string ReceiverUsername { get; set; }

        public string GroupId { get; set; }

        public ICollection<ChatMessageViewModel> ChatMessages { get; set; }
    }
}
