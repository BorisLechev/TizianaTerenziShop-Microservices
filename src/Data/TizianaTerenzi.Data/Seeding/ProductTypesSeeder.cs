namespace TizianaTerenzi.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore.Internal;
    using TizianaTerenzi.Data.Models;

    public class ProductTypesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var productTypes = new List<string>
            {
                "Fragrance",
                "Attar",
            };

            foreach (var productType in productTypes)
            {
                if (!dbContext.ProductTypes.Any(pt => pt.Name == productType))
                {
                    await dbContext.ProductTypes.AddAsync(
                        new ProductType
                        {
                            Name = productType,
                        });
                }
            }
        }
    }
}
