namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Data.Wishlist;
    using Xunit;

    public class WishlistServiceTests
    {
        [Fact]
        public async Task AddProductToTheWishlistTheResultShouldBeTrue()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            var service = new WishlistService(new EfDeletableEntityRepository<FavoriteProduct>(dbContext));

            var product = new Product
            {
                Name = "Test",
                Description = "Test",
                Price = 311,
                FragranceGroupId = 1,
                YearOfManufacture = 2016,
            };

            await dbContext.AddAsync(product);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await service.AddProductToTheWishlistAsync(product.Id, "1");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetAllProductsFromUsersWishlist()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            var service = new WishlistService(new EfDeletableEntityRepository<FavoriteProduct>(dbContext));

            var product = new Product
            {
                Name = "Test",
                Description = "Test",
                Price = 311,
                FragranceGroupId = 1,
                YearOfManufacture = 2016,
            };

            await dbContext.AddAsync(product);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await service.AddProductToTheWishlistAsync(product.Id, "1");
            var allProducts = await service.GetAllProductsFromUsersWishlistAsync("1");

            // Assert
            Assert.True(result);
            Assert.Single(allProducts);
        }

        [Fact]
        public async Task HasTheProductAlreadyAddedToTheWishlistTheResultShouldBeTrue()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            var service = new WishlistService(new EfDeletableEntityRepository<FavoriteProduct>(dbContext));

            var product = new Product
            {
                Name = "Test",
                Description = "Test",
                Price = 311,
                FragranceGroupId = 1,
                YearOfManufacture = 2016,
            };

            await dbContext.AddAsync(product);
            await dbContext.SaveChangesAsync();

            // Act
            var result1 = await service.AddProductToTheWishlistAsync(product.Id, "1");
            var result2 = await service.HasTheProductAlreadyAddedToTheWishlistAsync(product.Id, "1");

            // Assert
            Assert.True(result1);
            Assert.True(result2);
        }

        [Fact]
        public async Task HasTheProductAlreadyAddedToTheWishlistTheResultShouldBeFalse()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            var service = new WishlistService(new EfDeletableEntityRepository<FavoriteProduct>(dbContext));

            var product = new Product
            {
                Name = "Test",
                Description = "Test",
                Price = 311,
                FragranceGroupId = 1,
                YearOfManufacture = 2016,
            };

            await dbContext.AddAsync(product);
            await dbContext.SaveChangesAsync();

            // Act
            var result1 = await service.AddProductToTheWishlistAsync(product.Id, "1");
            var result2 = await service.HasTheProductAlreadyAddedToTheWishlistAsync(product.Id, "2");

            // Assert
            Assert.True(result1);
            Assert.False(result2);
        }

        [Fact]
        public async Task DeleteProductFromTheWishlistTheResultShouldBeTrue()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            var service = new WishlistService(new EfDeletableEntityRepository<FavoriteProduct>(dbContext));

            var product = new Product
            {
                Name = "Test",
                Description = "Test",
                Price = 311,
                FragranceGroupId = 1,
                YearOfManufacture = 2016,
            };

            await dbContext.AddAsync(product);
            await dbContext.SaveChangesAsync();

            // Act
            var result1 = await service.AddProductToTheWishlistAsync(product.Id, "1");
            var result2 = await service.DeleteProductFromTheWishlistAsync(product.Id, "1");
            var result3 = await service.DeleteProductFromTheWishlistAsync(product.Id, "1");

            // Assert
            Assert.True(result1);
            Assert.True(result2);
            Assert.False(result3);
        }
    }
}
