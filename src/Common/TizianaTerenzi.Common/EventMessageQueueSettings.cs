namespace TizianaTerenzi.Common
{
    public class EventMessageQueueSettings
    {
        public EventMessageQueueSettings(string host, string userName, string password)
        {
            this.Host = host ?? "localhost";
            this.UserName = userName ?? "guest";
            this.Password = password ?? "guest";
        }

        public string Host { get; }

        public string UserName { get; }

        public string Password { get; }
    }
}
