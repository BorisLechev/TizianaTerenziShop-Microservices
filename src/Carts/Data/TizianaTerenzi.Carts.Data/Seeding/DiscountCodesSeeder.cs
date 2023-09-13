namespace TizianaTerenzi.Carts.Data.Seeding
{
    using TizianaTerenzi.Carts.Data.Models;
    using TizianaTerenzi.Common.Data.Seeding;

    public class DiscountCodesSeeder : ISeeder<CartsDbContext>
    {
        public async Task SeedAsync(CartsDbContext dbContext, IServiceProvider serviceProvider)
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
                ExpiresOn = new DateTime(DateTime.UtcNow.Year, 12, 31, 23, 59, 59),
            });

            await dbContext.DiscountCodes.AddRangeAsync(discountCodeModels);
        }
    }
}
