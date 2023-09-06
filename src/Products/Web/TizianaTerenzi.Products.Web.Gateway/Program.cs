namespace TizianaTerenzi.Products.Web.Gateway
{
    using TizianaTerenzi.Common.Services.Identity;
    using TizianaTerenzi.Common.Web.Infrastructure.Middlewares;
    using TizianaTerenzi.Common.Web.Infrastructure.Extensions;
    using TizianaTerenzi.Products.Web.Gateway.Services;
    using TizianaTerenzi.Products.Web.Gateway.Services.Identity;
    using TizianaTerenzi.Products.Web.Gateway.Services.Products;

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
                .UseResponseCompression();

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddJwtTokenAuthentication(configuration)
                .AddScoped<ICurrentTokenService, CurrentTokenService>()
                .AddCustomResponseCompression()
                .AddControllers();

            services
                .AddExternalService<IIdentityService>(configuration)
                .AddExternalService<IProductsService>(configuration);
        }
    }
}
