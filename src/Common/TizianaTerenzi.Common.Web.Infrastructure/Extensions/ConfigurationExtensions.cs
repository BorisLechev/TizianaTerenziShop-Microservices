namespace TizianaTerenzi.Common.Web.Infrastructure.Extensions
{
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationExtensions
    {
        public static string GetDefaultConnectionString(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("DefaultConnection");
        }

        public static string GetCronJobsConnectionString(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("CronJobsConnection");
        }
    }
}
