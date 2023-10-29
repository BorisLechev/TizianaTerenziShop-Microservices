namespace TizianaTerenzi.Common.Web.Infrastructure
{
    public class ServiceEndpoints
    {
        public string Identity { get; private set; }

        public string Products { get; private set; }

        public string Carts { get; private set; }

        public string ProductsGateway { get; private set; }

        public string Notifications { get; private set; }

        public string Orders { get; private set; }

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
