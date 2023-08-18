namespace TizianaTerenzi.Products.Web
{
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Data.Seeding;
    using TizianaTerenzi.Common.Web.Infrastructure.Extensions;
    using TizianaTerenzi.Products.Data;
    using TizianaTerenzi.Products.Data.Repositories;
    using TizianaTerenzi.Products.Data.Seeding;
    using TizianaTerenzi.Products.Services.Data.Comments;
    using TizianaTerenzi.Products.Services.Data.FragranceGroups;
    using TizianaTerenzi.Products.Services.Data.Notes;
    using TizianaTerenzi.Products.Services.Data.Products;
    using TizianaTerenzi.Products.Services.Data.ProductTypes;
    using TizianaTerenzi.Products.Services.Data.Votes;
    using TizianaTerenzi.Products.Services.Data.Wishlist;

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

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMicroservice<ProductsDbContext>(configuration)
                .AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>))
                .AddScoped(typeof(IRepository<>), typeof(EfRepository<>))

                // -------Seeders--------
                .AddSingleton<ISeeder<ProductsDbContext>, FragranceGroupsSeeder>()
                .AddSingleton<ISeeder<ProductsDbContext>, ProductTypesSeeder>()
                .AddSingleton<ISeeder<ProductsDbContext>, NotesSeeder>()
                .AddSingleton<ISeeder<ProductsDbContext>, ProductsSeeder>()

                // -------Services------------
                .AddTransient<IProductsService, ProductsService>()
                .AddTransient<IProductTypesService, ProductTypesService>()
                .AddTransient<IFragranceGroupsService, FragranceGroupsService>()
                .AddTransient<INotesService, NotesService>()
                .AddTransient<ICommentsService, CommentsService>()
                .AddTransient<ICommentVotesService, CommentVotesService>()
                .AddTransient<IProductVotesService, ProductVotesService>()
                .AddTransient<IWishlistService, WishlistService>();
        }
    }
}
