namespace TizianaTerenzi.Products.Web
{
    using CloudinaryDotNet;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Data.Seeding;
    using TizianaTerenzi.Common.Web.Infrastructure.Extensions;
    using TizianaTerenzi.Products.Data;
    using TizianaTerenzi.Products.Data.Seeding;
    using TizianaTerenzi.Products.Services.Cloudinary;
    using TizianaTerenzi.Products.Web.Messages;

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
                .SeedDatabase<ProductsDbContext>();

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            Account account = new Account(
                    configuration["Cloudinary:CloudName"],
                    configuration["Cloudinary:ApiKey"],
                    configuration["Cloudinary:ApiSecret"]);

            Cloudinary cloudinary = new Cloudinary(account);

            services
                .AddMicroservice<ProductsDbContext>(configuration)
                .AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>))
                .AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
                .AddScoped<DbContext, ProductsDbContext>()

                // -------Seeders--------
                .AddSingleton<ISeeder<ProductsDbContext>, FragranceGroupsSeeder>()
                .AddSingleton<ISeeder<ProductsDbContext>, ProductTypesSeeder>()
                .AddSingleton<ISeeder<ProductsDbContext>, NotesSeeder>()
                .AddSingleton<ISeeder<ProductsDbContext>, ProductsSeeder>()
                .AddSingleton(cloudinary)

                // -------Services------------
                .AddTransient<ICloudinaryService, CloudinaryService>()
                .RegisterServices(configuration);

            services
                .AddMessageBroker(
                    configuration,
                    usePolling: true,
                    typeof(ProductCreatedConsumer),
                    typeof(ProductEditedConsumer),
                    typeof(ProductDeletedConsumer),
                    typeof(NoteCreatedConsumer),
                    typeof(ThePricesOfAllProductsAfterTheGeneralDiscountIsAppliedUpdatedConsumer),
                    typeof(ThePricesOfAllProductsAfterTheGeneralDiscountIsDisabledUpdatedConsumer),
                    typeof(AllProductsInTheUsersWishlistDeletedConsumer),
                    typeof(AllUserCommentsDeletedConsumer),
                    typeof(AllUserCommentVotesDeletedConsumer));
        }
    }
}
