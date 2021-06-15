namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Data.FragranceGroups;
    using Xunit;

    public class FragranceGroupsServiceTests
    {
        [Fact]
        public async Task GetAllFragranceGroupsShouldReturnSelectListItemArray()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var fragranceGroup = new FragranceGroup
            {
                Name = "Test",
            };

            await dbContext.FragranceGroups.AddAsync(fragranceGroup);
            await dbContext.SaveChangesAsync();

            var fragranceGroupsRepository = new EfRepository<FragranceGroup>(dbContext);
            var mockRepo = new Mock<IRepository<FragranceGroup>>();

            mockRepo.Setup(pv => pv.AllAsNoTracking())
                    .Returns(fragranceGroupsRepository.AllAsNoTracking());

            var service = new FragranceGroupsService(mockRepo.Object);

            // Act
            var result = await service.GetAllFragranceGroupsAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("1", result.First().Value);
            Assert.Equal(fragranceGroup.Name, result.First().Text);
        }
    }
}
