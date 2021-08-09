namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Data.Countries;
    using Xunit;

    public class CountriesServiceTests
    {
        [Fact]
        public async Task GetAllCountriesSuccessfully()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var newCountry = new Country
            {
                Name = "Bulgaria",
            };

            await dbContext.Countries.AddAsync(newCountry);
            await dbContext.SaveChangesAsync();

            var service = new CountriesService(new EfRepository<Country>(dbContext));

            // Act
            var result = await service.GetAllCountriesAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("1", result.First().Value);
            Assert.Equal(newCountry.Name, result.First().Text);
        }

        [Fact]
        public async Task GetCountryIdByNameSuccessfully()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            await dbContext.Countries.AddRangeAsync(
                new Country
                {
                    Name = "Bulgaria",
                },
                new Country
                {
                    Name = "Greece",
                });

            await dbContext.SaveChangesAsync();

            var service = new CountriesService(new EfRepository<Country>(dbContext));

            // Act
            var bulgarianId = await service.GetCountryIdByNameAsync("Bulgaria");
            var greekId = await service.GetCountryIdByNameAsync("Greece");

            // Assert
            Assert.Equal(1, bulgarianId);
            Assert.Equal(2, greekId);
        }
    }
}
