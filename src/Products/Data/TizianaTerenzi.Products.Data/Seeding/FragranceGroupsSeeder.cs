namespace TizianaTerenzi.Products.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TizianaTerenzi.Common.Data.Seeding;
    using TizianaTerenzi.Products.Data.Models;

    public class FragranceGroupsSeeder : ISeeder<ProductsDbContext>
    {
        public async Task SeedAsync(ProductsDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.FragranceGroups.Any())
            {
                return;
            }

            var fragranceGroups = new List<string>
            {
                "Aromatic Aquatic",
                "Aromatic Fruity",
                "Aromatic Green",
                "Aromatic Spicy",
                "Chypre Floral",
                "Chypre Fruity",
                "Citrus Aromatic",
                "Citrus Gourmand",
                "Floral Aldehyde",
                "Floral Aquatic",
                "Floral Fruity",
                "Floral Fruity Gourmand",
                "Floral Green",
                "Floral Woody Musk",
                "Leather",
                "Oriental Floral",
                "Oriental Fougere",
                "Oriental Spicy",
                "Oriental Vanilla",
                "Oriental Woody",
                "Woody",
                "Woody Aquatic",
                "Woody Aromatic",
                "Woody Chypre",
                "Woody Floral Musk",
                "Woody Spicy",
            };

            var fragranceGroupModels = fragranceGroups.Select(fg => new FragranceGroup { Name = fg });

            await dbContext.FragranceGroups.AddRangeAsync(fragranceGroupModels);
        }
    }
}
