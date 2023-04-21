namespace TizianaTerenzi.Products.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TizianaTerenzi.Common.Data.Seeding;
    using TizianaTerenzi.Products.Data.Models;

    public class ProductTypesSeeder : ISeeder<ProductsDbContext>
    {
        public async Task SeedAsync(ProductsDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ProductTypes.Any())
            {
                return;
            }

            var productTypes = new List<string>
            {
                "Fragrance",
                "Attar",
            };

            var productTypeModels = productTypes.Select(pt => new ProductType { Name = pt });

            await dbContext.ProductTypes.AddRangeAsync(productTypeModels);
        }
    }
}
