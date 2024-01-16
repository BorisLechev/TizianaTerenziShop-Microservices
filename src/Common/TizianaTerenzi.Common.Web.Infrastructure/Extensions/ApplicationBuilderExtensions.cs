namespace TizianaTerenzi.Common.Web.Infrastructure.Extensions
{
    using System;
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using TizianaTerenzi.Carts.Web.Models.Carts;
    using TizianaTerenzi.Common.Data.Seeding;
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Identity.Web.Models.Profile;
    using TizianaTerenzi.Orders.Web.Models.Orders;
    using TizianaTerenzi.Products.Web.Models.Products;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMicroservice(
            this IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .ConfigureAutoMapper()
                .UseHttpsRedirection()
                .UseRouting()
                .UseCors(options => options
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod())
                .UseAuthentication()
                .UseAuthorization()
                .UseResponseCompression()
                .UseStaticFiles();

            return app;
        }

        public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;
            var dbContext = serviceProvider.GetRequiredService<DbContext>();

            dbContext.Database.Migrate();

            return app;
        }

        public static IApplicationBuilder SeedDatabase<TDbContext>(this IApplicationBuilder app)
            where TDbContext : DbContext
        {
            // Seed data on application startup
            using var serviceScope = app.ApplicationServices.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;

            var dbContext = serviceScope.ServiceProvider.GetRequiredService<TDbContext>();

            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger($"{dbContext.GetType().Name}Seeder");

            var seeders = serviceProvider.GetServices<ISeeder<TDbContext>>();

            Task.Run(async () =>
            {
                foreach (var seeder in seeders)
                {
                    await seeder.SeedAsync(dbContext, serviceProvider);
                    await dbContext.SaveChangesAsync();

                    logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
                }
            })
            .GetAwaiter()
            .GetResult();

            return app;
        }

        public static IApplicationBuilder ConfigureAutoMapper(this IApplicationBuilder app)
        {
            AutoMapperConfig.RegisterMappings(
                                            typeof(ProductInListViewModel).GetTypeInfo().Assembly,
                                            typeof(DownloadPersonalDataViewModel).GetTypeInfo().Assembly,
                                            typeof(ProductsInTheCartViewModel).GetTypeInfo().Assembly,
                                            typeof(OrdersListingViewModel).GetTypeInfo().Assembly);

            return app;
        }
    }
}
