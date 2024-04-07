namespace TizianaTerenzi.Products.Web.Gateway
{
    using TizianaTerenzi.Common.Web.Infrastructure.Extensions;
    using TizianaTerenzi.Products.Web.Gateway.Services.Identity;
    using TizianaTerenzi.Products.Web.Gateway.Services.Orders;
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
                .UseGateway();

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddGateway(configuration);

            services
                .AddExternalService<IIdentityService>(configuration)
                .AddExternalService<IProductsService>(configuration)
                .AddExternalService<IOrdersService>(configuration);
        }
    }
}
