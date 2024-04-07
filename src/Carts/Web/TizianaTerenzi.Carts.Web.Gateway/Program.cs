namespace TizianaTerenzi.Carts.Web.Gateway
{
    using TizianaTerenzi.Carts.Web.Gateway.Services.Carts;
    using TizianaTerenzi.Carts.Web.Gateway.Services.Identity;
    using TizianaTerenzi.Common.Web.Infrastructure.Extensions;

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
                .AddExternalService<ICartsService>(configuration);
        }
    }
}
