namespace TizianaTerenzi.Identity.Web.Infrastructure
{
    using TizianaTerenzi.Identity.Data;
    using TizianaTerenzi.Identity.Data.Models;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityStorage(this IServiceCollection services)
        {
            services
                .AddIdentity<ApplicationUser, ApplicationRole>(IdentityOptionsProvider.GetIdentityOptions)
                .AddEntityFrameworkStores<IdentityDbContext>();

            return services;
        }
    }
}
