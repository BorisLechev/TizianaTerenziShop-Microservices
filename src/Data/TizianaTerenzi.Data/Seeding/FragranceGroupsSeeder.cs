namespace TizianaTerenzi.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;

    public class FragranceGroupsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
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

            foreach (var fragranceGroup in fragranceGroups)
            {
                if (!dbContext.FragranceGroups.Any(fg => fg.Name == fragranceGroup))
                {
                    await dbContext.FragranceGroups.AddAsync(
                        new FragranceGroup
                        {
                            Name = fragranceGroup,
                        });
                }
            }
        }
    }
}
