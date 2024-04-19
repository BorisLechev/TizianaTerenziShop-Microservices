namespace TizianaTerenzi.Common.Web.Infrastructure.Extensions
{
    using MassTransit;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class EventMessageConsumerRegistrationExtensions
    {
        public static IServiceCollection AddMessageBrokerConsumersWithReflection(
            this IServiceCollection services,
            IConfiguration configuration,
            bool usePolling = true)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(s => s.GetTypes())
                        .Where(type =>
                                    type.IsClass &&
                                    type.Name.EndsWith("Consumer") &&
                                    typeof(IConsumer).IsAssignableFrom(type))
                        .Select(consumer => consumer)
                        .ToArray();

            services.AddMessageBroker(
                configuration,
                usePolling: usePolling,
                types);

            return services;
        }
    }
}
