namespace TizianaTerenzi.Administration.Web
{
    using TizianaTerenzi.Administration.Data;
    using TizianaTerenzi.Administration.Data.Repositories;
    using TizianaTerenzi.Administration.Services.Data.Dashboard;
    using TizianaTerenzi.Administration.Services.Data.Notes;
    using TizianaTerenzi.Administration.Services.Data.Orders;
    using TizianaTerenzi.Administration.Services.Data.Products;
    using TizianaTerenzi.Administration.Services.Data.Users;
    using TizianaTerenzi.Administration.Web.Messages;
    using TizianaTerenzi.Common.Data.Repositories;
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

                // -------Seeders--------

                // -------Services------------
                .AddTransient<IOrdersService, OrdersService>()
                .AddTransient<IUsersService, UsersService>()
                .AddTransient<IDashboardService, DashboardService>()
                .AddTransient<IProductsService, ProductsService>()
                .AddTransient<INotesService, NotesService>();

            services
                .AddMessageBroker(
                    typeof(OrderAddedInAdminStatisticsConsumer),
                    typeof(UserAddedInAdminStatisticsConsumer));
        }
    }
}
