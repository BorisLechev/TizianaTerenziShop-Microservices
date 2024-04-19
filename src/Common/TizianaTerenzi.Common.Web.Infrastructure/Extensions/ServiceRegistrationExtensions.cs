namespace TizianaTerenzi.Common.Web.Infrastructure.Extensions
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TizianaTerenzi.Common.Services.ServiceRegistrationAttributes;

    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection RegisterServicesWithReflection(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(s => s.GetTypes())
                        .Where(type => type.IsDefined(typeof(SingletonRegistrationAttribute), false) ||
                                        type.IsDefined(typeof(ScopedRegistrationAttribute), false) ||
                                        type.IsDefined(typeof(TransientRegistrationAttribute), false))
                        .Select(type => new
                        {
                            Interface = type.GetInterface($"I{type.Name}"),
                            Implementation = type,
                        })
                        .ToList();

            Type scopedServiceRegistrationType = typeof(ScopedRegistrationAttribute);
            Type singletonServiceRegistrationType = typeof(SingletonRegistrationAttribute);
            Type transientServiceRegistrationType = typeof(TransientRegistrationAttribute);

            foreach (var type in types)
            {
                if (type.Implementation.IsDefined(transientServiceRegistrationType, false))
                {
                    services.AddTransient(type.Interface, type.Implementation);
                }
                else if (type.Implementation.IsDefined(scopedServiceRegistrationType, false))
                {
                    services.AddScoped(type.Interface, type.Implementation);
                }
                else if (type.Implementation.IsDefined(singletonServiceRegistrationType, false))
                {
                    services.AddSingleton(type.Interface, type.Implementation);
                }
            }

            return services;
        }
    }
}
