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
    using TizianaTerenzi.Services.Data.Notes;
    using TizianaTerenzi.Services.Data.Products;
    using TizianaTerenzi.Services.Mapping;
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

            var productType = new ProductType
            {
                Name = "Fragrance",
            };

            await dbContext.ProductTypes.AddAsync(productType);

            var fragranceGroup = new FragranceGroup
            {
                Name = "Chypre",
            };

            await dbContext.FragranceGroups.AddAsync(fragranceGroup);

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

            var productsRepository = new EfDeletableEntityRepository<Product>(dbContext);
            var mockProductsRepo = new Mock<IDeletableEntityRepository<Product>>();
            var cartRepository = new EfDeletableEntityRepository<Cart>(dbContext);
            var mockCartRepo = new Mock<IDeletableEntityRepository<Cart>>();
            var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);
            var mockOrdersRepo = new Mock<IDeletableEntityRepository<Order>>();
            var mockNotesService = new Mock<INotesService>();

            mockCartRepo.Setup(c => c.AddAsync(It.IsAny<Cart>()))
                    .Callback((Cart product) => cartRepository.AddAsync(product));
            mockCartRepo.Setup(pv => pv.SaveChangesAsync())
                   .Returns(cartRepository.SaveChangesAsync());

            var productsService = new ProductsService(mockProductsRepo.Object, mockNotesService.Object);
            var cartService = new CartService(mockCartRepo.Object, mockOrdersRepo.Object, null, null);

            // Act
            var result = await cartService.AddProductInTheCart(newProduct, "1");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CheckIfProductByUserIdExistInTheCart()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var productType = new ProductType
            {
                Name = "Fragrance",
            };

            await dbContext.ProductTypes.AddAsync(productType);

            var fragranceGroup = new FragranceGroup
            {
                Name = "Chypre",
            };

            await dbContext.FragranceGroups.AddAsync(fragranceGroup);

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

            var productsRepository = new EfDeletableEntityRepository<Product>(dbContext);
            var mockProductsRepo = new Mock<IDeletableEntityRepository<Product>>();
            var cartRepository = new EfDeletableEntityRepository<Cart>(dbContext);
            var mockCartRepo = new Mock<IDeletableEntityRepository<Cart>>();
            var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);
            var mockOrdersRepo = new Mock<IDeletableEntityRepository<Order>>();
            var mockNotesService = new Mock<INotesService>();

            var list = new List<Cart>();
            var mockList = list.AsQueryable().BuildMock();

            mockCartRepo.Setup(pv => pv.AllAsNoTracking())
                    .Returns(mockList.Object);
            mockCartRepo.Setup(c => c.AddAsync(It.IsAny<Cart>()))
                    .Callback((Cart product) => list.Add(product));
            mockCartRepo.Setup(pv => pv.SaveChangesAsync())
                   .Returns(cartRepository.SaveChangesAsync());

            var productsService = new ProductsService(mockProductsRepo.Object, mockNotesService.Object);
            var cartService = new CartService(mockCartRepo.Object, mockOrdersRepo.Object, null, null);

            // Assert
            Assert.True(await cartService.AddProductInTheCart(newProduct, "1"));
            Assert.True(await cartService.CheckIfProductByUserIdExistInTheCartAsync("1", 1));
            Assert.False(await cartService.CheckIfProductByUserIdExistInTheCartAsync("2", 1));
            Assert.False(await cartService.CheckIfProductByUserIdExistInTheCartAsync("2", 2));
            Assert.False(await cartService.CheckIfProductByUserIdExistInTheCartAsync("1", 2));
        }

        [Fact]
        public async Task IncreaseQuantityOfProductInTheCartSuccessfullyWithValidProductId()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var productType = new ProductType
            {
                Name = "Fragrance",
            };

            await dbContext.ProductTypes.AddAsync(productType);

            var fragranceGroup = new FragranceGroup
            {
                Name = "Chypre",
            };

            await dbContext.FragranceGroups.AddAsync(fragranceGroup);

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

            var productsRepository = new EfDeletableEntityRepository<Product>(dbContext);
            var mockProductsRepo = new Mock<IDeletableEntityRepository<Product>>();
            var cartRepository = new EfDeletableEntityRepository<Cart>(dbContext);
            var mockCartRepo = new Mock<IDeletableEntityRepository<Cart>>();
            var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);
            var mockOrdersRepo = new Mock<IDeletableEntityRepository<Order>>();
            var mockNotesService = new Mock<INotesService>();

            var list = new List<Cart>();
            var mockList = list.AsQueryable().BuildMock();

            mockCartRepo.Setup(pv => pv.All())
                    .Returns(mockList.Object);
            mockCartRepo.Setup(pv => pv.AllAsNoTracking())
                    .Returns(mockList.Object);
            mockCartRepo.Setup(c => c.AddAsync(It.IsAny<Cart>()))
                    .Callback((Cart product) => list.Add(product));
            mockCartRepo.Setup(pv => pv.SaveChangesAsync())
                   .Returns(cartRepository.SaveChangesAsync());

            var productsService = new ProductsService(mockProductsRepo.Object, mockNotesService.Object);
            var cartService = new CartService(mockCartRepo.Object, mockOrdersRepo.Object, null, null);
            AutoMapperConfig.RegisterMappings(typeof(ProductsInTheCartViewModel).Assembly);

            // Act
            var result = await cartService.AddProductInTheCart(newProduct, "1");
            var productInTheCartId = await cartService.GetProductInTheCartIdByProductIdAsync(1);

            // Assert
            Assert.True(result);
            Assert.NotNull(productInTheCartId);
            Assert.True(await cartService.IncreaseQuantity(productInTheCartId));
        }

        [Fact]
        public async Task ReduceQuantityOfProductInTheCartSuccessfullyWithValidProductIdAndWhooseQuantityIsAtLeast2Pieces()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var productType = new ProductType
            {
                Name = "Fragrance",
            };

            await dbContext.ProductTypes.AddAsync(productType);

            var fragranceGroup = new FragranceGroup
            {
                Name = "Chypre",
            };

            await dbContext.FragranceGroups.AddAsync(fragranceGroup);

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

            var productsRepository = new EfDeletableEntityRepository<Product>(dbContext);
            var mockProductsRepo = new Mock<IDeletableEntityRepository<Product>>();
            var cartRepository = new EfDeletableEntityRepository<Cart>(dbContext);
            var mockCartRepo = new Mock<IDeletableEntityRepository<Cart>>();
            var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);
            var mockOrdersRepo = new Mock<IDeletableEntityRepository<Order>>();
            var mockNotesService = new Mock<INotesService>();

            var list = new List<Cart>();
            var mockList = list.AsQueryable().BuildMock();

            mockCartRepo.Setup(pv => pv.All())
                    .Returns(mockList.Object);
            mockCartRepo.Setup(pv => pv.AllAsNoTracking())
                    .Returns(mockList.Object);
            mockCartRepo.Setup(c => c.AddAsync(It.IsAny<Cart>()))
                    .Callback((Cart product) => list.Add(product));
            mockCartRepo.Setup(pv => pv.SaveChangesAsync())
                   .Returns(cartRepository.SaveChangesAsync());

            var productsService = new ProductsService(mockProductsRepo.Object, mockNotesService.Object);
            var cartService = new CartService(mockCartRepo.Object, mockOrdersRepo.Object, null, null);
            AutoMapperConfig.RegisterMappings(typeof(ProductsInTheCartViewModel).Assembly);

            // Act
            var result = await cartService.AddProductInTheCart(newProduct, "1");
            var productInTheCartId = await cartService.GetProductInTheCartIdByProductIdAsync(1);

            // Assert
            Assert.True(result);
            Assert.NotNull(productInTheCartId);
            Assert.True(await cartService.IncreaseQuantity(productInTheCartId));
            Assert.True(await cartService.ReduceQuantity(productInTheCartId));
        }

        [Fact]
        public async Task ReduceQuantityOfProductInTheCartWithValidProductIdAndWhooseQuantityIsOnly1TheResultShouldBeFalse()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var productType = new ProductType
            {
                Name = "Fragrance",
            };

            await dbContext.ProductTypes.AddAsync(productType);

            var fragranceGroup = new FragranceGroup
            {
                Name = "Chypre",
            };

            await dbContext.FragranceGroups.AddAsync(fragranceGroup);

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

            var productsRepository = new EfDeletableEntityRepository<Product>(dbContext);
            var mockProductsRepo = new Mock<IDeletableEntityRepository<Product>>();
            var cartRepository = new EfDeletableEntityRepository<Cart>(dbContext);
            var mockCartRepo = new Mock<IDeletableEntityRepository<Cart>>();
            var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);
            var mockOrdersRepo = new Mock<IDeletableEntityRepository<Order>>();
            var mockNotesService = new Mock<INotesService>();

            var list = new List<Cart>();
            var mockList = list.AsQueryable().BuildMock();

            mockCartRepo.Setup(pv => pv.All())
                    .Returns(mockList.Object);
            mockCartRepo.Setup(pv => pv.AllAsNoTracking())
                    .Returns(mockList.Object);
            mockCartRepo.Setup(c => c.AddAsync(It.IsAny<Cart>()))
                    .Callback((Cart product) => list.Add(product));
            mockCartRepo.Setup(pv => pv.SaveChangesAsync())
                   .Returns(cartRepository.SaveChangesAsync());

            var productsService = new ProductsService(mockProductsRepo.Object, mockNotesService.Object);
            var cartService = new CartService(mockCartRepo.Object, mockOrdersRepo.Object, null, null);
            AutoMapperConfig.RegisterMappings(typeof(ProductsInTheCartViewModel).Assembly);

            // Act
            var result = await cartService.AddProductInTheCart(newProduct, "1");
            var productInTheCartId = await cartService.GetProductInTheCartIdByProductIdAsync(1);

            // Assert
            Assert.True(result);
            Assert.NotNull(productInTheCartId);
            Assert.False(await cartService.ReduceQuantity(productInTheCartId));
        }
    }
}
