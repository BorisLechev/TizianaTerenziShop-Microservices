namespace TizianaTerenzi.Identity.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Data.Seeding;
    using TizianaTerenzi.Identity.Data.Models;
    using TizianaTerenzi.Identity.Services.Data.Countries;

    public class RegularUserSeeder : ISeeder<IdentityDbContext>
    {
        private readonly ApplicationSettings applicationSettings;
        private readonly IdentitySettings identitySettings;

        public RegularUserSeeder(
            IOptions<ApplicationSettings> applicationSettings,
            IOptions<IdentitySettings> identitySettings)
        {
            this.applicationSettings = applicationSettings.Value;
            this.identitySettings = identitySettings.Value;
        }

        public async Task SeedAsync(IdentityDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (this.applicationSettings.SeedInitialRegularUser)
            {
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                if (userManager.Users.Any(u => u.Email == this.identitySettings.RegularUserMail) ||
                    userManager.Users.Any(u => u.UserName == this.identitySettings.RegularUserUserName))
                {
                    return;
                }

                var countriesService = serviceProvider.GetRequiredService<ICountriesService>();
                var bulgariaId = await countriesService.GetCountryIdByNameAsync("Bulgaria");

                var user = new ApplicationUser
                {
                    FirstName = "Petar",
                    LastName = "Petrov",
                    Email = this.identitySettings.RegularUserMail,
                    UserName = this.identitySettings.RegularUserUserName,
                    Town = "Sofia",
                    CountryId = bulgariaId,
                    PostalCode = "1000",
                    Address = "bul. Al. Tsankov 32",
                    IsBlocked = false,
                    PhoneNumber = "0888888888",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                };

                await userManager.CreateAsync(user, this.identitySettings.RegularUserPassword);
                await userManager.AddToRoleAsync(user, GlobalConstants.UserRoleName);
            }
        }
    }
}
