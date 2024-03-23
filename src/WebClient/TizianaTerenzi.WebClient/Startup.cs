namespace TizianaTerenzi.WebClient
{
    using System;
    using System.Globalization;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
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
    using TizianaTerenzi.Common.Web.Infrastructure.Extensions;
    using TizianaTerenzi.WebClient.Middlewares;
    using TizianaTerenzi.WebClient.Services.Administration;
    using TizianaTerenzi.WebClient.Services.Carts;
    using TizianaTerenzi.WebClient.Services.Identity;
    using TizianaTerenzi.WebClient.Services.Notifications;
    using TizianaTerenzi.WebClient.Services.Orders;
    using TizianaTerenzi.WebClient.Services.Products;

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

            services.AddSingleton(this.configuration);

            services
                .AddJwtTokenAuthentication(this.configuration);

            // Application services
            //services.AddScoped<IViewRenderService, ViewRenderService>();
            //services.AddScoped<IHtmlToPdfConverter, HtmlToPdfConverter>();
            services.AddScoped<ICurrentTokenService, CurrentTokenService>();

            services
                .AddExternalService<IIdentityService>(this.configuration)
                .AddExternalService<IProductsService>(this.configuration)
                .AddExternalService<IProductsGatewayService>(this.configuration)
                .AddExternalService<ICartsService>(this.configuration)
                .AddExternalService<ICartsGatewayService>(this.configuration)
                .AddExternalService<IOrdersService>(this.configuration)
                .AddExternalService<IAdministrationService>(this.configuration)
                .AddExternalService<INotificationsService>(this.configuration);

            //services
            //.AddRefitClient<IIdentityService>()
            //    .WithConfiguration(serviceEndpoints.Dealers);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
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

            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            StripeConfiguration.ApiKey = this.configuration["Stripe:SecretKey"];

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllerRoute(
                        "areaRoute",
                        "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapControllerRoute(
                        "productDetails",
                        "product/{id:int:min(1)}/{slug:required}",
                        new { controller = "Products", action = "Details" });
                    endpoints.MapControllerRoute(
                        "chatGroup",
                        "chat/with/{username?}/group/{groupId}",
                        new { area = "Chat", controller = "Chat", action = "Index" });
                    endpoints.MapControllerRoute(
                        "default",
                        "{controller=Home}/{action=Index}/{id?}");

                    endpoints.MapRazorPages();
                });
        }
    }
}
