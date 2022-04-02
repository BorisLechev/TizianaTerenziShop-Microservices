namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using MockQueryable.Moq;
    using Moq;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Data.Cart;
    using TizianaTerenzi.Services.Data.Orders;
    using TizianaTerenzi.Web.ViewModels.Orders;
    using Xunit;

    public class CartServiceTests
    {
        [Fact]
        public async Task AddProductInTheCartSuccessfully()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

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

            var cartRepository = new EfDeletableEntityRepository<Cart>(dbContext);
            var mockCartRepo = new Mock<IDeletableEntityRepository<Cart>>();

            mockCartRepo.Setup(c => c.AddAsync(It.IsAny<Cart>()))
                    .Callback((Cart product) => cartRepository.AddAsync(product));
            mockCartRepo.Setup(c => c.SaveChangesAsync())
                   .Returns(cartRepository.SaveChangesAsync());

            var cartService = new CartService(mockCartRepo.Object, null, null, null);

            // Act
            var result = await cartService.AddProductInTheCartAsync(newProduct, "1");

            // Assert
            mockCartRepo.Verify(x => x.AddAsync(It.IsAny<Cart>()), Times.Exactly(1));
            Assert.True(result);
        }

        [Fact]
        public async Task CheckIfProductByUserIdExistsInTheCart()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

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

            var cartRepository = new EfDeletableEntityRepository<Cart>(dbContext);
            var mockCartRepo = new Mock<IDeletableEntityRepository<Cart>>();

            var list = new List<Cart>();
            var mockList = list.BuildMock();

            mockCartRepo.Setup(c => c.AllAsNoTracking())
                    .Returns(mockList);
            mockCartRepo.Setup(c => c.AddAsync(It.IsAny<Cart>()))
                    .Callback((Cart product) => list.Add(product));
            mockCartRepo.Setup(c => c.SaveChangesAsync())
                   .Returns(cartRepository.SaveChangesAsync());

            var cartService = new CartService(mockCartRepo.Object, null, null, null);

            // Assert
            Assert.True(await cartService.AddProductInTheCartAsync(newProduct, "1"));
            mockCartRepo.Verify(x => x.AddAsync(It.IsAny<Cart>()), Times.Exactly(1));
            Assert.True(await cartService.CheckIfProductExistsInTheUsersCartAsync("1", 1));
            Assert.False(await cartService.CheckIfProductExistsInTheUsersCartAsync("2", 1));
            Assert.False(await cartService.CheckIfProductExistsInTheUsersCartAsync("2", 2));
            Assert.False(await cartService.CheckIfProductExistsInTheUsersCartAsync("1", 2));
        }

        [Fact]
        public async Task IncreaseQuantityOfProductInTheCartSuccessfullyWithValidProductId()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

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

            var cartRepository = new EfDeletableEntityRepository<Cart>(dbContext);
            var mockCartRepo = new Mock<IDeletableEntityRepository<Cart>>();

            var list = new List<Cart>();
            var mockList = list.BuildMock();

            mockCartRepo.Setup(c => c.All())
                    .Returns(mockList);
            mockCartRepo.Setup(c => c.AllAsNoTracking())
                    .Returns(mockList);
            mockCartRepo.Setup(c => c.AddAsync(It.IsAny<Cart>()))
                    .Callback((Cart product) => list.Add(product));
            mockCartRepo.Setup(c => c.SaveChangesAsync())
                   .Returns(cartRepository.SaveChangesAsync());

            var cartService = new CartService(mockCartRepo.Object, null, null, null);

            // Act
            var addProductInTheCartResult = await cartService.AddProductInTheCartAsync(newProduct, "1");
            var productInTheCartId = await cartService.GetProductInTheCartIdByProductIdAsync(1);

            // Assert
            mockCartRepo.Verify(x => x.AddAsync(It.IsAny<Cart>()), Times.Exactly(1));
            Assert.True(addProductInTheCartResult);
            Assert.NotNull(productInTheCartId);
            Assert.True(await cartService.IncreaseQuantityAsync(productInTheCartId));
        }

        [Fact]
        public async Task IncreaseQuantityOfProductInTheWithInValidProductIdTheResultShouldBeAnException()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

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

            var cartRepository = new EfDeletableEntityRepository<Cart>(dbContext);
            var mockCartRepo = new Mock<IDeletableEntityRepository<Cart>>();

            var list = new List<Cart>();
            var mockList = list.BuildMock();

            mockCartRepo.Setup(c => c.All())
                    .Returns(mockList);
            mockCartRepo.Setup(c => c.AllAsNoTracking())
                    .Returns(mockList);
            mockCartRepo.Setup(c => c.AddAsync(It.IsAny<Cart>()))
                    .Callback((Cart product) => list.Add(product));
            mockCartRepo.Setup(c => c.SaveChangesAsync())
                   .Returns(cartRepository.SaveChangesAsync());

            var cartService = new CartService(mockCartRepo.Object, null, null, null);

            // Act
            var addProductInTheCartResult = await cartService.AddProductInTheCartAsync(newProduct, "1");
            var productInTheCartId = await cartService.GetProductInTheCartIdByProductIdAsync(1);

            // Assert
            mockCartRepo.Verify(x => x.AddAsync(It.IsAny<Cart>()), Times.Exactly(1));
            Assert.True(addProductInTheCartResult);
            Assert.NotNull(productInTheCartId);
            await Assert.ThrowsAsync<NullReferenceException>(async () => await cartService.IncreaseQuantityAsync("2"));
        }

        [Fact]
        public async Task ReduceQuantityOfProductInTheCartSuccessfullyWithValidProductIdAndWhooseQuantityIsAtLeast2Pieces()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

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

            var cartRepository = new EfDeletableEntityRepository<Cart>(dbContext);
            var mockCartRepo = new Mock<IDeletableEntityRepository<Cart>>();

            var list = new List<Cart>();
            var mockList = list.BuildMock();

            mockCartRepo.Setup(c => c.All())
                    .Returns(mockList);
            mockCartRepo.Setup(c => c.AllAsNoTracking())
                    .Returns(mockList);
            mockCartRepo.Setup(c => c.AddAsync(It.IsAny<Cart>()))
                    .Callback((Cart product) => list.Add(product));
            mockCartRepo.Setup(c => c.SaveChangesAsync())
                   .Returns(cartRepository.SaveChangesAsync());

            var cartService = new CartService(mockCartRepo.Object, null, null, null);

            // Act
            var addProductInTheCartResult = await cartService.AddProductInTheCartAsync(newProduct, "1");
            var productInTheCartId = await cartService.GetProductInTheCartIdByProductIdAsync(1);

            // Assert
            mockCartRepo.Verify(x => x.AddAsync(It.IsAny<Cart>()), Times.Exactly(1));
            Assert.True(addProductInTheCartResult);
            Assert.NotNull(productInTheCartId);
            Assert.True(await cartService.IncreaseQuantityAsync(productInTheCartId));
            Assert.True(await cartService.ReduceQuantityAsync(productInTheCartId));
        }

        [Fact]
        public async Task ReduceQuantityOfProductInTheCartWithInValidProductIdAndWhooseQuantityIsAtLeast2PiecesShouldTwhowAnException()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

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

            var cartRepository = new EfDeletableEntityRepository<Cart>(dbContext);
            var mockCartRepo = new Mock<IDeletableEntityRepository<Cart>>();

            var list = new List<Cart>();
            var mockList = list.BuildMock();

            mockCartRepo.Setup(c => c.All())
                    .Returns(mockList);
            mockCartRepo.Setup(c => c.AllAsNoTracking())
                    .Returns(mockList);
            mockCartRepo.Setup(c => c.AddAsync(It.IsAny<Cart>()))
                    .Callback((Cart product) => list.Add(product));
            mockCartRepo.Setup(c => c.SaveChangesAsync())
                   .Returns(cartRepository.SaveChangesAsync());

            var cartService = new CartService(mockCartRepo.Object, null, null, null);

            // Act
            var addProductInTheCartResult = await cartService.AddProductInTheCartAsync(newProduct, "1");
            var productInTheCartId = await cartService.GetProductInTheCartIdByProductIdAsync(1);

            // Assert
            mockCartRepo.Verify(x => x.AddAsync(It.IsAny<Cart>()), Times.Exactly(1));
            Assert.True(addProductInTheCartResult);
            Assert.NotNull(productInTheCartId);
            Assert.True(await cartService.IncreaseQuantityAsync(productInTheCartId));
            await Assert.ThrowsAsync<NullReferenceException>(async () => await cartService.ReduceQuantityAsync("2"));
        }

        [Fact]
        public async Task ReduceQuantityOfProductInTheCartWithValidProductIdAndWhooseQuantityIsOnly1TheResultShouldBeFalse()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

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

            var cartRepository = new EfDeletableEntityRepository<Cart>(dbContext);
            var mockCartRepo = new Mock<IDeletableEntityRepository<Cart>>();

            var list = new List<Cart>();
            var mockList = list.BuildMock();

            mockCartRepo.Setup(c => c.All())
                    .Returns(mockList);
            mockCartRepo.Setup(c => c.AllAsNoTracking())
                    .Returns(mockList);
            mockCartRepo.Setup(c => c.AddAsync(It.IsAny<Cart>()))
                    .Callback((Cart product) => list.Add(product));
            mockCartRepo.Setup(c => c.SaveChangesAsync())
                   .Returns(cartRepository.SaveChangesAsync());

            var cartService = new CartService(mockCartRepo.Object, null, null, null);

            // Act
            var addProductInTheCartResult = await cartService.AddProductInTheCartAsync(newProduct, "1");
            var productInTheCartId = await cartService.GetProductInTheCartIdByProductIdAsync(1);

            // Assert
            mockCartRepo.Verify(x => x.AddAsync(It.IsAny<Cart>()), Times.Exactly(1));
            Assert.True(addProductInTheCartResult);
            Assert.NotNull(productInTheCartId);
            Assert.False(await cartService.ReduceQuantityAsync(productInTheCartId));
        }

        [Fact]
        public async Task SaveShippingDataWithValidModelTheResultShouldBeTrue()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var user = new ApplicationUser
            {
                FirstName = "FirstName2",
                LastName = "LastName2",
                UserName = "mail2@example.com",
                Email = "mail1@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var inputModel = new ShippingDataInputModel
            {
                FirstName = "K",
                LastName = "K",
                Address = "Bla",
                CountryId = 1,
                PhoneNumber = "0888888888",
                PostalCode = "1000",
                Town = "Sofia",
            };

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            userManager
                .Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            var service = new CartService(null, null, null, userManager.Object);

            // Act
            var result = await service.SaveShippingDataAsync(user, inputModel);
            var userResult = await dbContext.Users.SingleOrDefaultAsync(u => u.Id == user.Id);

            // Assert
            Assert.True(result);
            Assert.Equal(inputModel.PhoneNumber, userResult.PhoneNumber);
            Assert.Equal(inputModel.PostalCode, userResult.PostalCode);
            Assert.Equal(inputModel.Town, userResult.Town);
            Assert.Equal(inputModel.Address, userResult.Address);
            Assert.Equal(inputModel.CountryId, userResult.CountryId);
        }

        [Fact]
        public async Task GetAllProductsInTheCartTheResultShouldBeCorrectNumber()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

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

            var user = new ApplicationUser
            {
                FirstName = "FirstName2",
                LastName = "LastName2",
                UserName = "mail2@example.com",
                Email = "mail1@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var cartService = new CartService(
                new EfDeletableEntityRepository<Cart>(dbContext),
                null,
                null,
                null);

            // Act
            var addProductResult = await cartService.AddProductInTheCartAsync(newProduct, user.Id);
            var isThereAnyProductsInTheCart = await cartService.IsThereAnyProductsInTheUsersCartAsync(user.Id);

            var allProductsInTheCart = await cartService.GetAllProductsInTheCartByUserIdAsync(user.Id);

            // Assert
            Assert.True(addProductResult);
            Assert.True(isThereAnyProductsInTheCart);
            Assert.Single(allProductsInTheCart);
        }

        [Fact]
        public async Task CheckoutAllProductsInTheCartTheResultShouldBeTrue()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

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

            var user = new ApplicationUser
            {
                FirstName = "FirstName2",
                LastName = "LastName2",
                UserName = "mail2@example.com",
                Email = "mail1@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var mockOrderStatusesService = new Mock<IOrderStatusesService>();

            var cartService = new CartService(
                new EfDeletableEntityRepository<Cart>(dbContext),
                new EfDeletableEntityRepository<Order>(dbContext),
                mockOrderStatusesService.Object,
                null);

            // Act
            var addProductResult = await cartService.AddProductInTheCartAsync(newProduct, user.Id);
            var isThereAnyProductsInTheCart = await cartService.IsThereAnyProductsInTheUsersCartAsync(user.Id);

            var allProductsInTheCart = await cartService.GetAllProductsInTheCartByUserIdAsync(user.Id);
            var checkoutResult = await cartService.CheckoutAsync(user.Id);

            // Assert
            Assert.True(addProductResult);
            Assert.True(isThereAnyProductsInTheCart);
            Assert.Single(allProductsInTheCart);
            Assert.True(checkoutResult);
        }

        [Fact]
        public async Task DeleteProductByProductInTheCartIdTheResultShouldBeTrue()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

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

            var user = new ApplicationUser
            {
                FirstName = "FirstName2",
                LastName = "LastName2",
                UserName = "mail2@example.com",
                Email = "mail1@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var cartService = new CartService(
                new EfDeletableEntityRepository<Cart>(dbContext),
                null,
                null,
                null);

            // Act
            var addProductResult = await cartService.AddProductInTheCartAsync(newProduct, user.Id);
            var productInTheCartId = await cartService.GetProductInTheCartIdByProductIdAsync(newProduct.Id);
            var deleteProductResult = await cartService.DeleteProductInTheCartAsync(productInTheCartId);

            // Assert
            Assert.True(addProductResult);
            Assert.NotNull(productInTheCartId);
            Assert.True(deleteProductResult);
        }

        [Fact]
        public async Task DeleteAllProductsInTheCartTheResultShouldBeTrue()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

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

            var user = new ApplicationUser
            {
                FirstName = "FirstName2",
                LastName = "LastName2",
                UserName = "mail2@example.com",
                Email = "mail1@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var cartService = new CartService(
                new EfDeletableEntityRepository<Cart>(dbContext),
                null,
                null,
                null);

            // Act
            var addProductResult = await cartService.AddProductInTheCartAsync(newProduct, user.Id);
            var deleteProductsResult = await cartService.DeleteAllProductsInTheCartByUserIdAsync(user.Id);

            // Assert
            Assert.True(addProductResult);
            Assert.True(deleteProductsResult);
        }
    }
}
