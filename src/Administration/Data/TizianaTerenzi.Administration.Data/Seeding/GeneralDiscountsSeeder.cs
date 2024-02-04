namespace TizianaTerenzi.Administration.Data.Seeding
{
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Common.Data.Seeding;

    public class GeneralDiscountsSeeder : ISeeder<AdministrationDbContext>
    {
        public async Task SeedAsync(AdministrationDbContext dbContext, IServiceProvider serviceProvider)
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
