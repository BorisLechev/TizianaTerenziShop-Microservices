namespace TizianaTerenzi.Administration.Data.Seeding
{
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Common.Data.Seeding;

    public class DiscountCodesStatisticsSeeder : ISeeder<AdministrationDbContext>
    {
        public async Task SeedAsync(AdministrationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.DiscountCodeStatistics.Any())
            {
                return;
            }

            var discountCodes = new List<string>
            {
                "Test15",
            };

            var discountCodeModels = discountCodes.Select(dc => new DiscountCodeStatistics
            {
                Name = dc,
                Discount = 15,
                ExpiresOn = new DateTime(DateTime.UtcNow.Year, 12, 31, 23, 59, 59),
            });

            await dbContext.DiscountCodeStatistics.AddRangeAsync(discountCodeModels);
        }
    }
}
