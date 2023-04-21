namespace TizianaTerenzi.Identity.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Data.Seeding;
    using TizianaTerenzi.Identity.Data.Models;
    using TizianaTerenzi.Identity.Services.Data.Countries;

    public class RegularUserSeeder : ISeeder<IdentityDbContext>
    {
        public async Task SeedAsync(IdentityDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (userManager.Users.Any(u => u.Email == "petri@abv.bg") ||
                userManager.Users.Any(u => u.UserName == "petri"))
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
                PhoneNumberConfirmed = true,
            };

            await userManager.CreateAsync(user, "123456");
            await userManager.AddToRoleAsync(user, GlobalConstants.UserRoleName);
        }
    }
}
