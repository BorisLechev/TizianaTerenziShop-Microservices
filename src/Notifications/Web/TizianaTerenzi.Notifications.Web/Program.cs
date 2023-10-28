namespace TizianaTerenzi.Notifications.Web
{
    using TizianaTerenzi.Common.Services.Identity;
    using TizianaTerenzi.Common.Web.Infrastructure.Extensions;
    using TizianaTerenzi.Notifications.Services.Carts;
    using TizianaTerenzi.Notifications.Web.Hubs;
    using TizianaTerenzi.Notifications.Web.Infrastructure;
    using TizianaTerenzi.Notifications.Web.Messages;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseHttpsRedirection()
                .UseRouting()
                .UseCors(options => options
                    .WithOrigins("https://localhost:44319")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials())
                .UseAuthentication()
                .UseAuthorization()
                .UseResponseCompression()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapHub<NumberOfProductsInTheUsersCartHub>("/numberOfProductsInTheUsersCartHub");
                });

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddCors()
                .AddJwtTokenAuthentication(configuration, JwtConfiguration.BearerEvents)
                .AddScoped<ICurrentTokenService, CurrentTokenService>()
                .AddMessageBroker(typeof(ProductAddedInTheCartConsumer))
                .AddCustomResponseCompression()
                .AddSignalR();

            services
                .AddExternalService<ICartsService>(configuration);
        }
    }
}