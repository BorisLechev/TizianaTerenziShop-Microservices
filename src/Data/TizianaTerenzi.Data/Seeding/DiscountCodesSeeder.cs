namespace TizianaTerenzi.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;

    public class DiscountCodesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.DiscountCodes.Any())
            {
                return;
            }

            var discountCodes = new List<string>
            {
                "Test15",
            };

            var discountCodeModels = discountCodes.Select(dc => new DiscountCode
            {
                Name = dc,
                Discount = 15,
                ExpiresOn = new DateTime(2021, 12, 31, 23, 59, 59),
            });

            await dbContext.DiscountCodes.AddRangeAsync(discountCodeModels);
        }
    }
}
