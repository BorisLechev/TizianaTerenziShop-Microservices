namespace TizianaTerenzi.Administration.Web
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Administration.Data;
    using TizianaTerenzi.Administration.Data.Seeding;
    using TizianaTerenzi.Administration.Services.Data.Contacts;
    using TizianaTerenzi.Administration.Services.Data.Dashboard;
    using TizianaTerenzi.Administration.Services.Data.DiscountCodes;
    using TizianaTerenzi.Administration.Services.Data.GeneralDiscounts;
    using TizianaTerenzi.Administration.Services.Data.Notes;
    using TizianaTerenzi.Administration.Services.Data.Orders;
    using TizianaTerenzi.Administration.Services.Data.Products;
    using TizianaTerenzi.Administration.Services.Data.Subscribers;
    using TizianaTerenzi.Administration.Services.Data.UserPenalties;
    using TizianaTerenzi.Administration.Services.Data.Users;
    using TizianaTerenzi.Administration.Services.Location;
    using TizianaTerenzi.Administration.Web.Messages;
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

            app.MapControllers();

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
                .AddTransient<ILocationService, LocationService>()
                .AddTransient<IEmailSender>(x => new SendGridEmailSender(configuration["SendGrid:ApiKey"]))
                .AddTransient<IOrdersService, OrdersService>()
                .AddTransient<IUsersService, UsersService>()
                .AddTransient<IDashboardService, DashboardService>()
                .AddTransient<IProductsService, ProductsService>()
                .AddTransient<INotesService, NotesService>()
                .AddTransient<IGeneralDiscountsService, GeneralDiscountsService>()
                .AddTransient<IDiscountCodesService, DiscountCodesService>()
                .AddTransient<IUserPenaltiesService, UserPenaltiesService>()
                .AddTransient<IContactsService, ContactsService>()
                .AddTransient<ISubscribeService, SubscribeService>();

            services
                .AddMessageBroker(
                    typeof(OrderAddedInAdminStatisticsConsumer),
                    typeof(UserAddedInAdminStatisticsConsumer));
        }
    }
}
