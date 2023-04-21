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

    public class AdministratorSeeder : ISeeder<IdentityDbContext>
    {
        public async Task SeedAsync(IdentityDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            if (userManager.Users.Any(u => u.Email == "admin@admin.com") ||
                userManager.Users.Any(u => u.UserName == "admin") ||
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
                Email = "admin@admin.com",
                UserName = "admin",
                Town = "Varna",
                CountryId = bulgariaId,
                PostalCode = "9000",
                Address = "G. Rakovski 64",
                IsBlocked = false,
                PhoneNumber = "0888888888",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            await userManager.CreateAsync(user, "123456");
            await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
        }
    }
}
