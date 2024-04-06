namespace TizianaTerenzi.Orders.Web
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Data.Seeding;
    using TizianaTerenzi.Common.Web.Infrastructure.Extensions;
    using TizianaTerenzi.Orders.Data;
    using TizianaTerenzi.Orders.Data.Seeding;
    using TizianaTerenzi.Orders.Services.Data.Orders;
    using TizianaTerenzi.Orders.Web.Messages;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            app
                .UseMicroservice(builder.Environment)
                .MigrateDatabase()
                .SeedDatabase<OrdersDbContext>();

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
        {
            services
                .AddMicroservice<OrdersDbContext>(configuration)
                .AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>))
                .AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
                .AddScoped<DbContext, OrdersDbContext>()

                // -------Seeders--------
                .AddSingleton<ISeeder<OrdersDbContext>, OrderStatusesSeeder>()

                // -------Services------------
                .AddTransient<IOrderStatusesService, OrderStatusesService>()
                .AddTransient<IOrdersService, OrdersService>();

            services
                .AddMessageBroker(
                    configuration,
                    typeof(ProductsInTheUserCartHaveBeenOrderedConsumer),
                    typeof(OrderProcessedConsumer),
                    typeof(AllUserOrdersDeletedConsumer),
                    typeof(AllUserOrderProductsDeletedConsumer));
        }
    }
}