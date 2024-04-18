namespace TizianaTerenzi.Notifications.Web
{
    using Hangfire;
    using HealthChecks.UI.Client;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Services.Identity;
    using TizianaTerenzi.Common.Web.Infrastructure.Extensions;
    using TizianaTerenzi.Notifications.Data;
    using TizianaTerenzi.Notifications.Services.Data.Notifications;
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
                .ConfigureAutoMapper()
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
                .UseHangfireDashboard()
                .MigrateDatabase()
                .SeedDatabase<NotificationsDbContext>()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapHealthChecks("/health", new HealthCheckOptions
                    {
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                    });

                    endpoints.MapControllers();

                    endpoints.MapHub<NumberOfProductsInTheUsersCartHub>("/numberOfProductsInTheUsersCartHub");
                    endpoints.MapHub<UserStatusHub>("/userStatusHub");
                    endpoints.MapHub<ChatHub>("/chatHub");
                    endpoints.MapHub<NotificationHub>("/notificationHub");
                });

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddCors()
                .AddDatabase<NotificationsDbContext>(configuration)
                .AddApplicationSettings(configuration)
                .AddJwtTokenAuthentication(configuration, JwtConfiguration.BearerEvents)
                .AddHealth(configuration)
                .AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>))
                .AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
                .AddScoped<DbContext, NotificationsDbContext>()
                .AddScoped<ICurrentTokenService, CurrentTokenService>()

                // -------Seeders--------

                // -------Services------------
                .AddTransient<INotificationsService, NotificationsService>()
                .RegisterServices(configuration)

                .AddCustomResponseCompression()
                .AddSignalR();

            services.AddControllers();

            services
                .AddMessageBroker(
                    configuration,
                    usePolling: true,
                    typeof(AllUserNotificationsDeletedConsumer),
                    typeof(ProductsQuantityInTheUsersCartIncreasedConsumer),
                    typeof(ProductsQuantityInTheUsersCartReducedConsumer),
                    typeof(ProductsQuantityInTheUsersCartDeletedConsumer),
                    typeof(ProductQuantityInTheUsersCartDeletedConsumer),
                    typeof(NotificationsUpdatedWhenProductAddedInTheCartConsumer));
        }
    }
}