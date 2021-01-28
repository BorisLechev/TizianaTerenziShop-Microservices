namespace TizianaTerenzi.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;

    public class GeneralDiscountSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.GeneralDiscounts.Any())
            {
                return;
            }

            var generalDiscount = new GeneralDiscount
            {
                Percent = 0,
                IsActive = 0,
            };

            await dbContext.GeneralDiscounts.AddAsync(generalDiscount);
        }
    }
}
