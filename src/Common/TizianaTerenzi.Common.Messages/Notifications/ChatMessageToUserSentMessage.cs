namespace TizianaTerenzi.Common.Messages.Notifications
{
    public class ChatMessageToUserSentMessage
    {
        public string SendersUsername { get; set; }

        public string ReceiversUsername { get; set; }

        public string SanitizedMessage { get; set; }

        public string GroupId { get; set; }
    }
}
