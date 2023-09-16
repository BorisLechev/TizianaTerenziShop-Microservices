namespace TizianaTerenzi.Carts.Web
{
    using MassTransit;
    using TizianaTerenzi.Carts.Data;
    using TizianaTerenzi.Carts.Data.Repositories;
    using TizianaTerenzi.Carts.Data.Seeding;
    using TizianaTerenzi.Carts.Services.Data.Carts;
    using TizianaTerenzi.Carts.Services.Data.Discounts;
    using TizianaTerenzi.Carts.Web.Messages;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Data.Seeding;
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
                .UseMicroservice(app.Environment)
                .MigrateDatabase()
                .SeedDatabase<CartsDbContext>();

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMicroservice<CartsDbContext>(configuration)
                .AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>))
                .AddScoped(typeof(IRepository<>), typeof(EfRepository<>))

                // -------Seeders--------
                .AddSingleton<ISeeder<CartsDbContext>, DiscountCodesSeeder>()

                // -------Services------------
                .AddTransient<ICartsService, CartsService>()
                .AddTransient<IDiscountCodesService, DiscountCodesService>();

            services
                .AddMassTransit(mt =>
                {
                    mt.AddConsumer<ProductAddedInTheCartConsumer>();

                    // A Transport
                    mt.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.ConfigureEndpoints(context);
                    });
                });
        }
    }
}
