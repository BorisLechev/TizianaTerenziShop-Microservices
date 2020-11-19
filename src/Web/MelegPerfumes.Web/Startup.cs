namespace MelegPerfumes.Web
{
    using System;
    using System.Reflection;

    using CloudinaryDotNet;
    using MelegPerfumes.Data;
    using MelegPerfumes.Data.Common;
    using MelegPerfumes.Data.Common.Repositories;
    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Data.Repositories;
    using MelegPerfumes.Data.Seeding;
    using MelegPerfumes.Services;
    using MelegPerfumes.Services.Data;
    using MelegPerfumes.Services.Mapping;
    using MelegPerfumes.Services.Messaging;
    using MelegPerfumes.Web.ViewModels;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            Account account = new Account(
                this.configuration["Cloudinary:CloudName"],
                this.configuration["Cloudinary:ApiKey"],
                this.configuration["Cloudinary:ApiSecret"]);

            Cloudinary cloudinary = new Cloudinary(account);

            services.AddSingleton(cloudinary);

            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services.Configure<CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.IsEssential = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Error403";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/login";
                options.LogoutPath = "/logout";
            });

            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;
            });

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()); // CSRF
            });
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddAuthentication()
            .AddGoogle(options =>
            {
                IConfigurationSection googleAuthNSection =
                    this.configuration.GetSection("Google");

                options.ClientId = googleAuthNSection["ClientId"];
                options.ClientSecret = googleAuthNSection["ClientSecret"];
            })
            .AddFacebook(options =>
            {
                options.AppId = this.configuration["FacebookSettings:AppId"];
                options.AppSecret = this.configuration["FacebookSettings:AppSecret"];
                options.AccessDeniedPath = "/AccessDeniedPathInfo";
            });

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddResponseCaching();
            services.AddResponseCompression(opt => opt.EnableForHttps = true);

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ISubscribeService, SubscribeService>();
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IDiscountCodesService, DiscountCodesService>();
            services.AddTransient<IOrderStatusesService, OrderStatusesService>();
            services.AddTransient<INotesService, NotesService>();
            services.AddTransient<IProductTypesService, ProductTypesService>();
            services.AddTransient<IFragranceGroupsService, FragranceGroupsService>();
            services.AddTransient<ICommentsService, CommentsService>();
            services.AddTransient<IVotesService, VotesService>();
            services.AddTransient<IPersonalDataService, PersonalDataService>();
            services.AddTransient<IOrdersService, OrdersService>();
            services.AddTransient<IUrlGenerator, UrlGenerator>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseResponseCompression();

            app.UseStatusCodePages();
            app.UseStatusCodePagesWithReExecute("/Error{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute(
                            "Product details",
                            "product/{name}/{id}",
                            new { controller = "Products", action = "Details" });
                        endpoints.MapControllerRoute(
                            "areaRoute",
                            "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute(
                            "default",
                            "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
