namespace TizianaTerenzi.Carts.Web
{
    using TizianaTerenzi.Carts.Data;
    using TizianaTerenzi.Carts.Data.Repositories;
    using TizianaTerenzi.Carts.Data.Seeding;
    using TizianaTerenzi.Carts.Services.Data.Carts;
    using TizianaTerenzi.Carts.Services.Data.Countries;
    using TizianaTerenzi.Carts.Services.Data.Discounts;
    using TizianaTerenzi.Carts.Services.Data.GeneralDiscounts;
    using TizianaTerenzi.Carts.Web.Messages;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Data.Seeding;
    using TizianaTerenzi.Common.Web.Infrastructure.Extensions;
    using TizianaTerenzi.Notifications.Web.Hubs;

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
                .SeedDatabase<CartsDbContext>()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapHub<NumberOfProductsInTheUsersCartHub>("/numberOfProductsInTheUsersCartHub");
                });

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
                .AddSingleton<ISeeder<CartsDbContext>, CountriesSeeder>()

                // -------Services------------
                .AddTransient<ICartsService, CartsService>()
                .AddTransient<IDiscountCodesService, DiscountCodesService>()
                .AddTransient<ICountriesService, CountriesService>()
                .AddTransient<IGeneralDiscountsService, GeneralDiscountsService>();

            services
                .AddMessageBroker(
                    typeof(ProductAddedInTheCartConsumer),
                    typeof(ThePricesOfAllProductsInTheCartAfterTheGeneralDiscountIsAppliedUpdatedConsumer),
                    typeof(ThePricesOfAllProductsInTheCartAfterTheGeneralDiscountIsDisabledUpdatedConsumer),
                    typeof(DiscountCodeCreatedConsumer),
                    typeof(DiscountCodeDeletedConsumer),
                    typeof(ProductInAllCartsEditedConsumer),
                    typeof(ProductInAllCartsDeletedConsumer))
                .AddSignalR();
        }
    }
}
