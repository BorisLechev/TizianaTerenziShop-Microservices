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
            var productSortings = new List<string>()
            {
                "All Products",
                "Product: A - Z",
                "Price: Ascending",
                "Price: Descending",
                "Year of Release: Ascending",
                "Year of Release: Descending",
            };

            foreach (var sortingName in productSortings)
            {
                if (!dbContext.ProductSortings.Any(s => s.Name == sortingName))
                {
                    await dbContext.ProductSortings.AddAsync(
                        new ProductSorting
                        {
                            Name = sortingName,
                        });
                }
            }
        }
    }
}
