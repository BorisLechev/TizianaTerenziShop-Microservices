namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Data.Discounts;
    using TizianaTerenzi.Web.ViewModels.DiscountCodes;
    using Xunit;

    public class DiscountCodesServiceTests
    {
        [Fact]
        public async Task WhenAdminTriesToCreate2DiscountCodesWithSameNameTheResultShouldBeFalse()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var inputModel = new CreateDiscountCodeInputModel
            {
                Name = "Test",
                Discount = 10,
                ExpiresOn = DateTime.UtcNow.AddMonths(1),
            };

            var discountCode = new DiscountCode
            {
                Name = "Test",
                Discount = 10,
                ExpiresOn = DateTime.UtcNow.AddMonths(1),
            };

            await dbContext.DiscountCodes.AddAsync(discountCode);
            await dbContext.SaveChangesAsync();

            var discountCodesService = new DiscountCodesService(
                new EfDeletableEntityRepository<DiscountCode>(dbContext),
                null);

            // Assert
            Assert.False(await discountCodesService.CreateDiscountCodeAsync(inputModel));
            Assert.False(await discountCodesService.CreateDiscountCodeAsync(inputModel));
        }

        [Fact]
        public async Task WhenAdminTriesToDeleteTheDiscountCodesWithValidDiscountCodeIdShouldWorkCorrectly()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var discountCode = new DiscountCode
            {
                Name = "Test",
                Discount = 10,
                ExpiresOn = DateTime.UtcNow.AddMonths(1),
            };

            await dbContext.DiscountCodes.AddAsync(discountCode);
            await dbContext.SaveChangesAsync();

            var discountCodesService = new DiscountCodesService(
                new EfDeletableEntityRepository<DiscountCode>(dbContext),
                null);

            // Assert
            Assert.True(await discountCodesService.CheckIfThereIsSuchaDiscountAsync(discountCode.Name));
            Assert.True(await discountCodesService.DeleteDiscountCodeAsync(1));
            Assert.False(await discountCodesService.DeleteDiscountCodeAsync(2));
        }

        [Fact]
        public async Task ModifyThePricesAfterAppliedDiscountCodeShouldWorkCorrectly()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var discountCode = new DiscountCode
            {
                Name = "Test",
                Discount = 10,
                ExpiresOn = DateTime.UtcNow.AddMonths(1),
            };

            await dbContext.DiscountCodes.AddAsync(discountCode);
            await dbContext.SaveChangesAsync();

            var newProduct = new Product
            {
                Name = "Kiki",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Picture = "https://res.cloudinary.com/pictures-storage/image/upload/v1612213773/product_images/y6mh1xtdt7lkmgvrt3gy.jpg",
                Price = 320,
                PriceWithGeneralDiscount = 320,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2015,
            };

            await dbContext.Products.AddAsync(newProduct);
            await dbContext.SaveChangesAsync();

            var productInTheCart = new Cart
            {
                ProductId = newProduct.Id,
                ProductPriceWithDiscountCode = newProduct.PriceWithGeneralDiscount,
                UserId = "1",
                Quantity = 1,
            };

            await dbContext.Carts.AddAsync(productInTheCart);
            await dbContext.SaveChangesAsync();

            var discountCodesService = new DiscountCodesService(
                new EfDeletableEntityRepository<DiscountCode>(dbContext),
                new EfDeletableEntityRepository<Cart>(dbContext));

            // Act
            var modifyPrices = await discountCodesService.ModifyThePricesAfterAppliedDiscountCodeAsync(discountCode.Name, "1");

            // Assert
            Assert.True(modifyPrices);
            Assert.Equal(288, productInTheCart.ProductPriceWithDiscountCode);
        }

        [Fact]
        public async Task ModifyThePricesAfterDeletedDiscountCodeShouldWorkCorrectly()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var discountCode = new DiscountCode
            {
                Name = "Test",
                Discount = 10,
                ExpiresOn = DateTime.UtcNow.AddMonths(1),
            };

            await dbContext.DiscountCodes.AddAsync(discountCode);
            await dbContext.SaveChangesAsync();

            var newProduct = new Product
            {
                Name = "Kiki",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Picture = "https://res.cloudinary.com/pictures-storage/image/upload/v1612213773/product_images/y6mh1xtdt7lkmgvrt3gy.jpg",
                Price = 320,
                PriceWithGeneralDiscount = 320,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2015,
            };

            await dbContext.Products.AddAsync(newProduct);
            await dbContext.SaveChangesAsync();

            var productInTheCart = new Cart
            {
                ProductId = newProduct.Id,
                ProductPriceWithDiscountCode = newProduct.PriceWithGeneralDiscount,
                UserId = "1",
                Quantity = 1,
            };

            await dbContext.Carts.AddAsync(productInTheCart);
            await dbContext.SaveChangesAsync();

            var discountCodesService = new DiscountCodesService(
                new EfDeletableEntityRepository<DiscountCode>(dbContext),
                new EfDeletableEntityRepository<Cart>(dbContext));

            // Act
            var modifyThePricesResult = await discountCodesService.ModifyThePricesAfterAppliedDiscountCodeAsync(discountCode.Name, "1");

            // Assert
            Assert.True(modifyThePricesResult);
            Assert.Equal(288, productInTheCart.ProductPriceWithDiscountCode);
            Assert.Equal("Kiki", productInTheCart.Product.Name);

            productInTheCart.DiscountCodeId = 1;
            await dbContext.SaveChangesAsync();

            Assert.True(await discountCodesService.ModifyThePricesAfterDeletedDiscountCodeAsync("1"));
            Assert.Equal(320, productInTheCart.ProductPriceWithDiscountCode);
            Assert.Equal("Kiki", productInTheCart.Product.Name);
        }

        [Fact]
        public async Task GetDiscountCodeByNameShouldWorkCorrectly()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var discountCode = new DiscountCode
            {
                Name = "Test",
                Discount = 10,
                ExpiresOn = DateTime.UtcNow.AddMonths(1),
            };

            await dbContext.DiscountCodes.AddAsync(discountCode);
            await dbContext.SaveChangesAsync();

            var discountCodesService = new DiscountCodesService(
                new EfDeletableEntityRepository<DiscountCode>(dbContext),
                null);

            // Act
            var result = await discountCodesService.GetDiscountCodeByNameAsync(discountCode.Name);

            // Assert
            Assert.Equal(discountCode.Name, result.Name);
            Assert.Equal(discountCode.Discount, result.Discount);
            Assert.Equal(discountCode.ExpiresOn, result.ExpiresOn);
        }

        [Fact]
        public async Task GetAllDiscountCodesTheResultShouldBeArrayOfDiscountCodes()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var discountCode = new DiscountCode
            {
                Name = "Test",
                Discount = 10,
                ExpiresOn = DateTime.UtcNow.AddMonths(1),
            };

            await dbContext.DiscountCodes.AddAsync(discountCode);
            await dbContext.SaveChangesAsync();

            var discountCodesService = new DiscountCodesService(
                new EfDeletableEntityRepository<DiscountCode>(dbContext),
                null);

            // Act
            var discountCodes = await discountCodesService.GetAllDiscountCodesAsync<DiscountCodesListingViewModel>();
            var firstDiscountCode = discountCodes.First();

            // Assert
            Assert.Single(discountCodes);
            Assert.Equal(discountCode.Name, firstDiscountCode.Name);
            Assert.Equal(discountCode.Discount, firstDiscountCode.Discount);
            Assert.Equal(discountCode.ExpiresOn, firstDiscountCode.ExpiresOn);
        }
    }
}
