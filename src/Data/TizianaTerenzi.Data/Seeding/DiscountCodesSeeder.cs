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
            var discountCodes = new List<string>
            {
                "Test15",
                "Christmas15",
            };

            foreach (var discountCode in discountCodes)
            {
                if (!dbContext.DiscountCodes.Any(dc => dc.Name == discountCode))
                {
                    await dbContext.DiscountCodes
                        .AddAsync(new DiscountCode
                        {
                            Name = discountCode,
                            Discount = 15,
                            ExpiresOn = new DateTime(2020, 12, 26, 23, 59, 59),
                        });
                }
            }
        }
    }
}
