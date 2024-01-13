namespace TizianaTerenzi.Carts.Data.Seeding
{
    using TizianaTerenzi.Carts.Data.Models;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Data.Seeding;

    public class CountriesSeeder : ISeeder<CartsDbContext>
    {
        public async Task SeedAsync(CartsDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Countries.Any())
            {
                return;
            }

            var countries = ListOfCountries.Countries.Select(c => new Country { Name = c });

            await dbContext.AddRangeAsync(countries);
        }
    }
}
