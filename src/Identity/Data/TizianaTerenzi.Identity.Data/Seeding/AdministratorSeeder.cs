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

    public class AdministratorSeeder : ISeeder<IdentityDbContext>
    {
        private readonly IdentitySettings identitySettings;

        public AdministratorSeeder(
            IOptions<IdentitySettings> identitySettings)
        {
            this.identitySettings = identitySettings.Value;
        }

        public async Task SeedAsync(IdentityDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            if (userManager.Users.Any(u => u.Email == this.identitySettings.AdminMail) ||
                userManager.Users.Any(u => u.UserName == this.identitySettings.AdminUserName) ||
                await roleManager.RoleExistsAsync(GlobalConstants.AdministratorRoleName) == false)
            {
                return;
            }

            var countriesService = serviceProvider.GetRequiredService<ICountriesService>();
            var bulgariaId = await countriesService.GetCountryIdByNameAsync("Bulgaria");

            var user = new ApplicationUser
            {
                FirstName = "Admin",
                LastName = "Admin",
                Email = this.identitySettings.AdminMail,
                UserName = this.identitySettings.AdminUserName,
                Town = "Varna",
                CountryId = bulgariaId,
                PostalCode = "9000",
                Address = "G. Rakovski 64",
                IsBlocked = false,
                PhoneNumber = "0888888888",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            await userManager.CreateAsync(user, this.identitySettings.AdminPassword);
            await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
        }
    }
}
