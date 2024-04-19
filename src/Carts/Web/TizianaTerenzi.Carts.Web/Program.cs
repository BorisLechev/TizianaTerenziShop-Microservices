namespace TizianaTerenzi.Carts.Web
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Carts.Data;
    using TizianaTerenzi.Carts.Data.Seeding;
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

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMicroservice<CartsDbContext>(configuration)
                .AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>))
                .AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
                .AddScoped<DbContext, CartsDbContext>()

                // -------Seeders--------
                .AddSingleton<ISeeder<CartsDbContext>, DiscountCodesSeeder>()
                .AddSingleton<ISeeder<CartsDbContext>, CountriesSeeder>()

                // -------Services------------
                .RegisterServicesWithReflection(configuration);

            services
                .AddMessageBrokerConsumersWithReflection(
                    configuration,
                    usePolling: true);
        }
    }
}
