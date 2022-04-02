namespace TizianaTerenzi.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Countries;

    public class RegularUserSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (userManager.Users.Any(u => u.Email == "petri@abv.bg"))
            {
                return;
            }

            var countriesService = serviceProvider.GetRequiredService<ICountriesService>();
            var bulgariaId = await countriesService.GetCountryIdByNameAsync("Bulgaria");

            var user = new ApplicationUser
            {
                FirstName = "Petar",
                LastName = "Petrov",
                Email = "petri@abv.bg",
                UserName = "petri",
                Town = "Sofia",
                CountryId = bulgariaId,
                PostalCode = "1000",
                Address = "bul. Al. Tsankov 32",
                IsBlocked = false,
                PhoneNumber = "0888888888",
                EmailConfirmed = true,
            };

            await userManager.CreateAsync(user, "123456");
            await userManager.AddToRoleAsync(user, GlobalConstants.UserRoleName);
        }
    }
}
