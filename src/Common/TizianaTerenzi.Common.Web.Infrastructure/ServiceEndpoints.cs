namespace TizianaTerenzi.Common.Web.Infrastructure
{
    public class ServiceEndpoints
    {
        public string Identity { get; private set; }

        public string Products { get; private set; }

        public string Carts { get; private set; }

        public string IdentityGateway { get; private set; }

        public string Notifications { get; private set; }

        public string NotificationsForSignalR { get; set; }

        public string Orders { get; private set; }

        public string CartsGateway { get; private set; }

        public string Administration { get; private set; }

        public string Watchdog { get; private set; }

        public string this[string service]
            => this.GetType()
                .GetProperties()
                .Where(pr => string
                    .Equals(pr.Name, service, StringComparison.CurrentCultureIgnoreCase))
                .Select(pr => (string)pr.GetValue(this))
                .FirstOrDefault()
                ?? throw new InvalidOperationException(
                    $"External service with name '{service}' does not exists.");
    }
}
