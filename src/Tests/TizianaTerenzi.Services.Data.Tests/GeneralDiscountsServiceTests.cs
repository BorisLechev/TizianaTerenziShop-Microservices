namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Data.Discounts;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.WebClient.ViewModels.GeneralDiscounts;
    using Xunit;

    public class GeneralDiscountsServiceTests
    {
        [Fact]
        public async Task GetGeneralDiscountShouldReturnGeneralDiscountIfFound()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var generalDiscount = new GeneralDiscount
            {
                Percent = 15,
                IsActive = GeneralDiscountCondition.Inactive,
            };

            await dbContext.GeneralDiscounts.AddAsync(generalDiscount);

            var generalDiscountsRepository = new EfRepository<GeneralDiscount>(dbContext);
            var mockGeneralDiscountsRepo = new Mock<IRepository<GeneralDiscount>>();

            mockGeneralDiscountsRepo.Setup(gd => gd.All())
                    .Returns(generalDiscountsRepository.All());
            mockGeneralDiscountsRepo.Setup(gd => gd.SaveChangesAsync())
                   .Returns(generalDiscountsRepository.SaveChangesAsync());

            var discountCodesService = new GeneralDiscountsService(mockGeneralDiscountsRepo.Object);

            // Act
            var result = await discountCodesService.GetGeneralDiscountAsync();

            // Assert
            Assert.Equal(GeneralDiscountCondition.Inactive, result.IsActive);
            Assert.Equal(15, result.Percent);
        }

        [Fact]
        public async Task GetGeneralDiscountViewModelShouldReturnGeneralDiscountIfFound()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var generalDiscount = new GeneralDiscount
            {
                Percent = 15,
                IsActive = GeneralDiscountCondition.Inactive,
            };

            await dbContext.GeneralDiscounts.AddAsync(generalDiscount);

            var generalDiscountsRepository = new EfRepository<GeneralDiscount>(dbContext);
            var mockGeneralDiscountsRepo = new Mock<IRepository<GeneralDiscount>>();

            mockGeneralDiscountsRepo.Setup(gd => gd.All())
                    .Returns(generalDiscountsRepository.All());
            mockGeneralDiscountsRepo.Setup(gd => gd.SaveChangesAsync())
                   .Returns(generalDiscountsRepository.SaveChangesAsync());

            var discountCodesService = new GeneralDiscountsService(mockGeneralDiscountsRepo.Object);
            AutoMapperConfig.RegisterMappings(typeof(GeneralDiscountViewModel).GetTypeInfo().Assembly);

            // Act
            var result = await discountCodesService.GetGeneralDiscountAsync<GeneralDiscountViewModel>();

            // Assert
            Assert.Equal((int)generalDiscount.IsActive, result.IsActive);
            Assert.Equal(15, result.PercentId);
        }

        [Fact]
        public async Task ApplyDiscountToAllProductsShouldWorkCorrectly()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var generalDiscount = new GeneralDiscount
            {
                Percent = 15,
                IsActive = GeneralDiscountCondition.Inactive,
            };

            await dbContext.GeneralDiscounts.AddAsync(generalDiscount);

            var generalDiscountsRepository = new EfRepository<GeneralDiscount>(dbContext);
            var mockGeneralDiscountsRepo = new Mock<IRepository<GeneralDiscount>>();

            mockGeneralDiscountsRepo.Setup(gd => gd.All())
                    .Returns(generalDiscountsRepository.All());
            mockGeneralDiscountsRepo.Setup(gd => gd.SaveChangesAsync())
                   .Returns(generalDiscountsRepository.SaveChangesAsync());

            var discountCodesService = new GeneralDiscountsService(mockGeneralDiscountsRepo.Object);

            // Act
            var result = await discountCodesService.ApplyDiscountToAllProductsAsync(10);

            // Assert
            Assert.True(result);
            Assert.Equal(GeneralDiscountCondition.Active, generalDiscount.IsActive);
            Assert.Equal(10, generalDiscount.Percent);
        }

        [Fact]
        public async Task DisableDiscountToAllProductsShouldWorkCorrectly()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var generalDiscount = new GeneralDiscount
            {
                Percent = 15,
                IsActive = GeneralDiscountCondition.Active,
            };

            await dbContext.GeneralDiscounts.AddAsync(generalDiscount);

            var generalDiscountsRepository = new EfRepository<GeneralDiscount>(dbContext);
            var mockGeneralDiscountsRepo = new Mock<IRepository<GeneralDiscount>>();

            mockGeneralDiscountsRepo.Setup(gd => gd.All())
                    .Returns(generalDiscountsRepository.All());
            mockGeneralDiscountsRepo.Setup(gd => gd.SaveChangesAsync())
                   .Returns(generalDiscountsRepository.SaveChangesAsync());

            var discountCodesService = new GeneralDiscountsService(mockGeneralDiscountsRepo.Object);

            // Act
            var result = await discountCodesService.DisableDiscountToAllProductsAsync();

            // Assert
            Assert.True(result);
            Assert.Equal(GeneralDiscountCondition.Inactive, generalDiscount.IsActive);
            Assert.Equal(15, generalDiscount.Percent);
        }
    }
}
