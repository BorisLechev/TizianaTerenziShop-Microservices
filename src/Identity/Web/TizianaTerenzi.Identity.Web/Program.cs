namespace TizianaTerenzi.Identity.Web
{
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Data.Seeding;
    using TizianaTerenzi.Common.Web.Infrastructure.Extensions;
    using TizianaTerenzi.Identity.Data;
    using TizianaTerenzi.Identity.Data.Repositories;
    using TizianaTerenzi.Identity.Data.Seeding;
    using TizianaTerenzi.Identity.Services.Data.Chat;
    using TizianaTerenzi.Identity.Services.Data.Countries;
    using TizianaTerenzi.Identity.Services.Data.Identity;
    using TizianaTerenzi.Identity.Services.Data.Profile;
    using TizianaTerenzi.Identity.Services.Data.UserPenalties;
    using TizianaTerenzi.Identity.Services.Data.Users;
    using TizianaTerenzi.Identity.Services.Location;
    using TizianaTerenzi.Identity.Services.Scrapers;
    using TizianaTerenzi.Identity.Web.Infrastructure;
    using TizianaTerenzi.Identity.Web.Messages;

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
                .SeedDatabase<IdentityDbContext>();

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMicroservice<IdentityDbContext>(configuration)
                .AddIdentityStorage()
                .AddScoped(typeof(IDeletableEntityRepository<>), typeof(TizianaTerenzi.Identity.Data.Repositories.EfDeletableEntityRepository<>))
                .AddScoped(typeof(IRepository<>), typeof(TizianaTerenzi.Identity.Data.Repositories.EfRepository<>))

                // -------Seeders--------
                .AddSingleton<ISeeder<IdentityDbContext>, CountriesSeeder>()
                .AddSingleton<ISeeder<IdentityDbContext>, RolesSeeder>()
                .AddSingleton<ISeeder<IdentityDbContext>, AdministratorSeeder>()
                .AddSingleton<ISeeder<IdentityDbContext>, RegularUserSeeder>()
                //.AddSingleton<ISeeder<IdentityDbContext>, EmojisSeeder>()

                // -------Services------------
                .AddTransient<ILocationService, LocationService>()
                .AddTransient<ICountriesService, CountriesService>()
                .AddTransient<IIdentityService, IdentityService>()
                .AddTransient<IProfileService, ProfileService>()
                .AddTransient<ITokenGeneratorService, TokenGeneratorService>()
                .AddTransient<IUsersService, UsersService>()
                .AddTransient<IUserPenaltiesService, UserPenaltiesService>()
                .AddTransient<IUnicodeEmojiScraperService, UnicodeEmojiScraperService>()
                .AddTransient<IChatService, ChatService>()
                .AddTransient<IEmojisService, EmojisService>();

            services
                .AddMessageBroker(
                    typeof(UserProfileDataUpdatedAfterProductsInTheCartHaveBeenOrderedConsumer),
                    typeof(UserInRoleAddedConsumer),
                    typeof(UserBlockedConsumer),
                    typeof(UserUnblockedConsumer),
                    typeof(ChatMessageToUserSentConsumer));
        }
    }
}
