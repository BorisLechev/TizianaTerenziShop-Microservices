namespace TizianaTerenzi.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;

    public class ProductSortingSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ProductSortings.Any())
            {
                return;
            }

            var productSortings = new List<string>()
            {
                "All Products",
                "Product: A - Z",
                "Price: Ascending",
                "Price: Descending",
                "Year of Release: Ascending",
                "Year of Release: Descending",
            };

            var productSortingModels = productSortings.Select(ps => new ProductSorting { Name = ps });

            await dbContext.AddRangeAsync(productSortingModels);
        }
    }
}
