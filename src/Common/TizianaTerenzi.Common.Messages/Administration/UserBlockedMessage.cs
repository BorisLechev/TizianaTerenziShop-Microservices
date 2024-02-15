namespace TizianaTerenzi.Common.Messages.Administration
{
    public class UserBlockedMessage
    {
        public string UserId { get; set; }

        public string ReasonToBeBlocked { get; set; }
    }
}
