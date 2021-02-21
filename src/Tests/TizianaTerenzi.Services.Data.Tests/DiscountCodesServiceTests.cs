namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MockQueryable.Moq;
    using Moq;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Data.Cart;
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

            var discountCodesRepository = new EfDeletableEntityRepository<DiscountCode>(dbContext);
            var mockDiscountCodesRepo = new Mock<IDeletableEntityRepository<DiscountCode>>();
            var list = new List<DiscountCode>();
            var mockList = list.AsQueryable().BuildMock();

            mockDiscountCodesRepo.Setup(dc => dc.AllAsNoTracking())
                    .Returns(mockList.Object);
            mockDiscountCodesRepo.Setup(dc => dc.SaveChangesAsync())
                   .Returns(discountCodesRepository.SaveChangesAsync());
            mockDiscountCodesRepo.Setup(dc => dc.AddAsync(It.IsAny<DiscountCode>()))
                    .Callback((DiscountCode discountCode) => list.Add(discountCode));

            var discountCodesService = new DiscountCodesService(mockDiscountCodesRepo.Object, null);

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

            var discountCodesRepository = new EfDeletableEntityRepository<DiscountCode>(dbContext);
            var mockDiscountCodesRepo = new Mock<IDeletableEntityRepository<DiscountCode>>();

            mockDiscountCodesRepo.Setup(dc => dc.All())
                    .Returns(discountCodesRepository.All());
            mockDiscountCodesRepo.Setup(dc => dc.SaveChangesAsync())
                   .Returns(discountCodesRepository.SaveChangesAsync());
            mockDiscountCodesRepo.Setup(dc => dc.Delete(It.IsAny<DiscountCode>()))
                    .Callback((DiscountCode discountCode) => discountCodesRepository.Delete(discountCode));

            var discountCodesService = new DiscountCodesService(mockDiscountCodesRepo.Object, null);

            // Assert
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

            var newProduct = new Product
            {
                Name = "Kiki",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Picture = "https://res.cloudinary.com/pictures-storage/image/upload/v1612213773/product_images/y6mh1xtdt7lkmgvrt3gy.jpg",
                Price = 320,
                PriceWithDiscount = 320,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2015,
            };

            await dbContext.Products.AddAsync(newProduct);

            var productInTheCart = new ProductInTheCart
            {
                ProductId = newProduct.Id,
                ProductPriceAfterDiscount = newProduct.PriceWithDiscount,
                UserId = "1",
                Quantity = 1,
            };

            await dbContext.ProductsInTheCart.AddAsync(productInTheCart);

            var discountCodesRepository = new EfDeletableEntityRepository<DiscountCode>(dbContext);
            var mockDiscountCodesRepo = new Mock<IDeletableEntityRepository<DiscountCode>>();
            var productsInTheCartRepository = new EfDeletableEntityRepository<ProductInTheCart>(dbContext);
            var mockProductsInTheCartRepo = new Mock<IDeletableEntityRepository<ProductInTheCart>>();
            var cartRepository = new EfDeletableEntityRepository<ProductInTheCart>(dbContext);
            var mockCartRepo = new Mock<IDeletableEntityRepository<ProductInTheCart>>();

            mockDiscountCodesRepo.Setup(dc => dc.All())
                    .Returns(discountCodesRepository.All());
            mockProductsInTheCartRepo.Setup(dc => dc.All())
                    .Returns(productsInTheCartRepository.All());
            mockProductsInTheCartRepo.Setup(dc => dc.SaveChangesAsync())
                   .Returns(productsInTheCartRepository.SaveChangesAsync());

            var discountCodesService = new DiscountCodesService(mockDiscountCodesRepo.Object, mockProductsInTheCartRepo.Object);
            var cartService = new CartService(mockCartRepo.Object, null, null, null);

            // Assert
            Assert.True(await discountCodesService.ModifyThePricesAfterAppliedDiscountCodeAsync("Test", "1"));
            Assert.Equal(288, productInTheCart.ProductPriceAfterDiscount);
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

            var newProduct = new Product
            {
                Name = "Kiki",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Picture = "https://res.cloudinary.com/pictures-storage/image/upload/v1612213773/product_images/y6mh1xtdt7lkmgvrt3gy.jpg",
                Price = 320,
                PriceWithDiscount = 320,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2015,
            };

            await dbContext.Products.AddAsync(newProduct);

            var productInTheCart = new ProductInTheCart
            {
                ProductId = newProduct.Id,
                ProductPriceAfterDiscount = newProduct.PriceWithDiscount,
                UserId = "1",
                Quantity = 1,
            };

            await dbContext.ProductsInTheCart.AddAsync(productInTheCart);

            var discountCodesRepository = new EfDeletableEntityRepository<DiscountCode>(dbContext);
            var mockDiscountCodesRepo = new Mock<IDeletableEntityRepository<DiscountCode>>();
            var productsInTheCartRepository = new EfDeletableEntityRepository<ProductInTheCart>(dbContext);
            var mockProductsInTheCartRepo = new Mock<IDeletableEntityRepository<ProductInTheCart>>();
            var cartRepository = new EfDeletableEntityRepository<ProductInTheCart>(dbContext);
            var mockCartRepo = new Mock<IDeletableEntityRepository<ProductInTheCart>>();

            mockDiscountCodesRepo.Setup(dc => dc.All())
                    .Returns(discountCodesRepository.All());
            mockProductsInTheCartRepo.Setup(dc => dc.All())
                    .Returns(productsInTheCartRepository.All());
            mockProductsInTheCartRepo.Setup(dc => dc.SaveChangesAsync())
                   .Returns(productsInTheCartRepository.SaveChangesAsync());

            var discountCodesService = new DiscountCodesService(mockDiscountCodesRepo.Object, mockProductsInTheCartRepo.Object);
            var cartService = new CartService(mockCartRepo.Object, null, null, null);

            // Assert
            Assert.True(await discountCodesService.ModifyThePricesAfterAppliedDiscountCodeAsync("Test", "1"));
            Assert.Equal(288, productInTheCart.ProductPriceAfterDiscount);
            Assert.Equal("Kiki", productInTheCart.Product.Name);

            productInTheCart.DiscountCodeId = 1;
            await dbContext.SaveChangesAsync();

            Assert.True(await discountCodesService.ModifyThePricesAfterDeletedDiscountCodeAsync("1"));
            Assert.Equal(320, productInTheCart.ProductPriceAfterDiscount);
            Assert.Equal("Kiki", productInTheCart.Product.Name);
        }
    }
}
