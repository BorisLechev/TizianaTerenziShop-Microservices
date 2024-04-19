namespace TizianaTerenzi.Administration.Web
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Administration.Data;
    using TizianaTerenzi.Administration.Data.Seeding;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Data.Seeding;
    using TizianaTerenzi.Common.Services.Messaging;
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
                .SeedDatabase<AdministrationDbContext>();

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMicroservice<AdministrationDbContext>(configuration)
                .AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>))
                .AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
                .AddScoped<DbContext, AdministrationDbContext>()

                // -------Seeders--------
                .AddSingleton<ISeeder<AdministrationDbContext>, GeneralDiscountsSeeder>()
                .AddSingleton<ISeeder<AdministrationDbContext>, DiscountCodesStatisticsSeeder>()

                // -------Services------------
                .AddTransient<IEmailSender>(x => new SendGridEmailSender(configuration["SendGrid:ApiKey"]))
                .RegisterServicesWithReflection(configuration);

            services
                .AddMessageBrokerConsumersWithReflection(
                    configuration,
                    usePolling: true);
        }
    }
}
