namespace TizianaTerenzi.Identity.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Data.Seeding;
    using TizianaTerenzi.Identity.Data.Models;

    public class CountriesSeeder : ISeeder<IdentityDbContext>
    {
        public async Task SeedAsync(IdentityDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Countries.Any())
            {
                return;
            }

            var countryModels = ListOfCountries.Countries.Select(c => new Country { Name = c });

            await dbContext.AddRangeAsync(countryModels);
        }
    }
}
