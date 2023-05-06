namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.IO;
    using System.Reflection;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Common;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Cloudinary;
    using TizianaTerenzi.Services.Data.Cart;
    using TizianaTerenzi.Services.Data.Chat;
    using TizianaTerenzi.Services.Data.Comments;
    using TizianaTerenzi.Services.Data.Countries;
    using TizianaTerenzi.Services.Data.Dashboard;
    using TizianaTerenzi.Services.Data.Discounts;
    using TizianaTerenzi.Services.Data.FragranceGroups;
    using TizianaTerenzi.Services.Data.Notes;
    using TizianaTerenzi.Services.Data.Notifications;
    using TizianaTerenzi.Services.Data.Orders;
    using TizianaTerenzi.Services.Data.Products;
    using TizianaTerenzi.Services.Data.Profile;
    using TizianaTerenzi.Services.Data.Subscribe;
    using TizianaTerenzi.Services.Data.UserPenalties;
    using TizianaTerenzi.Services.Data.Users;
    using TizianaTerenzi.Services.Data.Votes;
    using TizianaTerenzi.Services.Data.Wishlist;
    using TizianaTerenzi.Services.Location;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.Services.Messaging;
    using TizianaTerenzi.Services.PDF;
    using TizianaTerenzi.Services.SlugGenerator;
    using TizianaTerenzi.WebClient.ViewModels;
    using TizianaTerenzi.WebClient.ViewModels.Products;
    using Z.EntityFramework.Extensions;

    public abstract class BaseServiceTests
    {
        protected BaseServiceTests()
        {
            this.Configuration = this.SetConfiguration();
            var services = this.SetServices();
            this.RegisterMappings();
            this.ServiceProvider = services.BuildServiceProvider();
            this.DbContext = this.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            this.Seed();
        }

        protected IServiceProvider ServiceProvider { get; set; }

        protected ApplicationDbContext DbContext { get; set; }

        protected IConfigurationRoot Configuration { get; set; }

        private ServiceCollection SetServices()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();
            services.AddSingleton<IConfiguration>(this.Configuration);

            // Application services
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddScoped<IHtmlToPdfConverter, HtmlToPdfConverter>();
            services.AddTransient<IEmailSender>(x => new SendGridEmailSender(this.Configuration["SendGrid:ApiKey"]));
            services.AddTransient<ISubscribeService, SubscribeService>();
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IDiscountCodesService, DiscountCodesService>();
            services.AddTransient<IOrderStatusesService, OrderStatusesService>();
            services.AddTransient<INotesService, NotesService>();
            services.AddTransient<IProductTypesService, ProductTypesService>();
            services.AddTransient<IFragranceGroupsService, FragranceGroupsService>();
            services.AddTransient<ICommentsService, CommentsService>();
            services.AddTransient<ICommentVotesService, CommentVotesService>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IOrdersService, OrdersService>();
            services.AddTransient<ICountriesService, CountriesService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IWishlistService, WishlistService>();
            services.AddTransient<IGeneralDiscountsService, GeneralDiscountsService>();
            services.AddTransient<IProductVotesService, ProductVotesService>();
            services.AddTransient<IChatService, ChatService>();
            services.AddTransient<INotificationsService, NotificationsService>();
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IUserPenaltiesService, UserPenaltiesService>();
            services.AddTransient<ISlugGeneratorService, SlugGeneratorService>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();

            EntityFrameworkManager.ContextFactory = context =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
                return new ApplicationDbContext(optionsBuilder.Options);
            };

            return services;
        }

        private IConfigurationRoot SetConfiguration()
        {
            return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(
                 path: "appsettings.json",
                 optional: false,
                 reloadOnChange: true)
           .Build();
        }

        private void RegisterMappings()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(ProductDetailsViewModel).GetTypeInfo().Assembly,
                typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }

        private void Seed()
        {
            this.DbContext.Products.AddRange(
                new Product
                {
                    Name = "Kiki",
                    Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Price = 320,
                    PriceWithGeneralDiscount = 320,
                    FragranceGroupId = 2,
                    ProductTypeId = 2,
                    YearOfManufacture = 2015,
                },
                new Product
                {
                    Name = "Lili",
                    Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Price = 420,
                    PriceWithGeneralDiscount = 420,
                    FragranceGroupId = 3,
                    ProductTypeId = 3,
                    YearOfManufacture = 2016,
                },
                new Product
                {
                    Name = "Jiji",
                    Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Price = 520,
                    PriceWithGeneralDiscount = 520,
                    FragranceGroupId = 4,
                    ProductTypeId = 4,
                    YearOfManufacture = 2017,
                },
                new Product
                {
                    Name = "Fifi",
                    Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Price = 520,
                    PriceWithGeneralDiscount = 520,
                    FragranceGroupId = 5,
                    ProductTypeId = 5,
                    YearOfManufacture = 2018,
                },
                new Product
                {
                    Name = "Bibi",
                    Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Price = 620,
                    PriceWithGeneralDiscount = 620,
                    FragranceGroupId = 6,
                    ProductTypeId = 6,
                    YearOfManufacture = 2019,
                },
                new Product
                {
                    Name = "Hihi",
                    Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Price = 720,
                    PriceWithGeneralDiscount = 720,
                    FragranceGroupId = 7,
                    ProductTypeId = 7,
                    YearOfManufacture = 2020,
                });
            this.DbContext.SaveChanges();
        }
    }
}
