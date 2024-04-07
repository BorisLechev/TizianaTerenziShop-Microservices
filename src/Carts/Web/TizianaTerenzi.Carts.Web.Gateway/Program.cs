namespace TizianaTerenzi.Carts.Web.Gateway
{
    using HealthChecks.UI.Client;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using TizianaTerenzi.Carts.Web.Gateway.Services.Carts;
    using TizianaTerenzi.Carts.Web.Gateway.Services.Identity;
    using TizianaTerenzi.Common.Services.Identity;
    using TizianaTerenzi.Common.Web.Infrastructure.Extensions;
    using TizianaTerenzi.Common.Web.Infrastructure.Middlewares;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            app
                .ConfigureAutoMapper()
                .UseHttpsRedirection()
                .UseRouting()
                .UseJwtHeaderAuthenticationMiddleware()
                .UseAuthorization()
                .UseResponseCompression()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapHealthChecks("/health", new HealthCheckOptions
                    {
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                    });

                    endpoints.MapControllers();
                });

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddJwtTokenAuthentication(configuration)
                .AddHealth(configuration, includeSqlServer: false, includeRabbitMq: false)
                .AddScoped<ICurrentTokenService, CurrentTokenService>()
                .AddCustomResponseCompression()
                .AddControllers();

            services
                .AddExternalService<IIdentityService>(configuration)
                .AddExternalService<ICartsService>(configuration);
        }
    }
}
