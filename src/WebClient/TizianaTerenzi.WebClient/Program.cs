namespace TizianaTerenzi.WebClient
{
    using System;
    using System.Globalization;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Stripe;
    using TizianaTerenzi.Common.Services.Identity;
    using TizianaTerenzi.Common.Web.Infrastructure;
    using TizianaTerenzi.Common.Web.Infrastructure.Extensions;
    using TizianaTerenzi.WebClient.Middlewares;
    using TizianaTerenzi.WebClient.Services.Administration;
    using TizianaTerenzi.WebClient.Services.Carts;
    using TizianaTerenzi.WebClient.Services.Identity;
    using TizianaTerenzi.WebClient.Services.Notifications;
    using TizianaTerenzi.WebClient.Services.Orders;
    using TizianaTerenzi.WebClient.Services.Products;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();
            Configure(app);

            app.Run();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("bg"),
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    //options.CheckConsentNeeded = context => true;
                    options.CheckConsentNeeded = context => false;
                    //options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services.Configure<CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.IsEssential = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Error403";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/identity/account/login";
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

            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddResponseCaching();
            services.AddResponseCompression(opt => opt.EnableForHttps = true);

            services.AddSingleton(configuration);

            services
                .AddJwtTokenAuthentication(configuration);

            // Application services
            //services.AddScoped<IViewRenderService, ViewRenderService>();
            //services.AddScoped<IHtmlToPdfConverter, HtmlToPdfConverter>();
            services.AddScoped<ICurrentTokenService, CurrentTokenService>();

            services
                .AddExternalService<IIdentityService>(configuration)
                .AddExternalService<IProductsService>(configuration)
                .AddExternalService<IIdentityGatewayService>(configuration)
                .AddExternalService<ICartsService>(configuration)
                .AddExternalService<ICartsGatewayService>(configuration)
                .AddExternalService<IOrdersService>(configuration)
                .AddExternalService<IAdministrationService>(configuration)
                .AddExternalService<INotificationsService>(configuration);

            //services
            //.AddRefitClient<IIdentityService>()
            //    .WithConfiguration(serviceEndpoints.Dealers);

            services.Configure<ServiceEndpoints>(configuration.GetSection(nameof(ServiceEndpoints)));

            StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        private static void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.UseJwtCookieAuthenticationMiddleware();

            app.UseAuthorization();

            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            app.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute("productDetails", "product/{id:int:min(1)}/{slug:required}", new { controller = "Products", action = "Details" });
            app.MapControllerRoute("chatGroup", "chat/with/{username?}/group/{groupId}", new { area = "Chat", controller = "Chat", action = "Index" });
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
        }
    }
}
