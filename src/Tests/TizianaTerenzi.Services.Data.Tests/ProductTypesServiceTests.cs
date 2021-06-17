namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Data.Products;
    using Xunit;

    public class ProductTypeServiceTests : BaseServiceTests
    {
        private IProductTypesService Service => this.ServiceProvider.GetRequiredService<IProductTypesService>();

        [Fact]
        public async Task CreateProductTypeShouldReturnTrue()
        {
            var productType = new ProductType
            {
                Name = "Test",
            };

            // Act
            var result = await this.Service.CreateProductTypeAsync(productType);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetAllProductTypesShouldReturnSelectListItemArray()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var productType = new ProductType
            {
                Name = "Test",
            };

            await dbContext.ProductTypes.AddAsync(productType);
            await dbContext.SaveChangesAsync();

            var productTypesRepository = new EfRepository<ProductType>(dbContext);
            var mockRepo = new Mock<IRepository<ProductType>>();

            mockRepo.Setup(pt => pt.AllAsNoTracking())
                    .Returns(productTypesRepository.AllAsNoTracking());

            var service = new ProductTypesService(mockRepo.Object);

            // Act
            var result = await service.GetAllProductTypesAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("1", result.First().Value);
            Assert.Equal(productType.Name, result.First().Text);
        }
    }
}
