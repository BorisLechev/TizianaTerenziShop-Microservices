namespace TizianaTerenzi.Web
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Security.Claims;
    using System.Text.Json;

    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.OAuth;
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
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Common;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Data.Seeding;
    using TizianaTerenzi.Services;
    using TizianaTerenzi.Services.Data;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.Services.Messaging;
    using TizianaTerenzi.Web.ViewModels;

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
            })
            .AddOAuth("GitHub", options =>
            {
                options.ClientId = this.configuration["GitHub:ClientId"];
                options.ClientSecret = this.configuration["GitHub:ClientSecret"];
                options.CallbackPath = new PathString("/login-github");

                options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
                options.TokenEndpoint = "https://github.com/login/oauth/access_token";
                options.UserInformationEndpoint = "https://api.github.com/user";

                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                options.ClaimActions.MapJsonKey("urn:github:login", "login");

                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        var request =
                            new HttpRequestMessage(System.Net.Http.HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        request.Headers.Authorization =
                            new AuthenticationHeaderValue("Bearer", context.AccessToken);

                        var response = await context.Backchannel.SendAsync(
                            request,
                            HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();

                        var user = JsonSerializer.Deserialize<JsonElement>(await response.Content.ReadAsStringAsync());

                        context.RunClaimActions(user);
                    },
                };
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
                            "areaRoute",
                            "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute(
                            "details",
                            "product/{id:int:min(1)}/{slug:required}",
                            new { controller = "Products", action = "Details" });
                        endpoints.MapControllerRoute(
                            "details",
                            "product/{id:int:min(1)}",
                            new { controller = "Products", action = "Details" });
                        endpoints.MapControllerRoute(
                            "default",
                            "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
