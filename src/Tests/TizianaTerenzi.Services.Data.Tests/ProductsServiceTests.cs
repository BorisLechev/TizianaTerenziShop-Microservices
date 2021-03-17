namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Cloudinary;
    using TizianaTerenzi.Services.Data.Notes;
    using TizianaTerenzi.Services.Data.Products;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.Web.ViewModels.Products;
    using Xunit;

    public class ProductsServiceTests
    {
        [Fact]
        public async Task GetProductsByPageCorrectly()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            AutoMapperConfig.RegisterMappings(typeof(ProductInListViewModel).Assembly);

            await dbContext.Products.AddRangeAsync(
                new Product
                {
                    Name = "Kiki",
                    Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Price = 320,
                    PriceWithDiscount = 320,
                    FragranceGroupId = 2,
                    ProductTypeId = 2,
                    YearOfManufacture = 2015,
                },
                new Product
                {
                    Name = "Lili",
                    Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Price = 420,
                    PriceWithDiscount = 420,
                    FragranceGroupId = 3,
                    ProductTypeId = 3,
                    YearOfManufacture = 2016,
                },
                new Product
                {
                    Name = "Jiji",
                    Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Price = 520,
                    PriceWithDiscount = 520,
                    FragranceGroupId = 4,
                    ProductTypeId = 4,
                    YearOfManufacture = 2017,
                },
                new Product
                {
                    Name = "Fifi",
                    Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Price = 520,
                    PriceWithDiscount = 520,
                    FragranceGroupId = 5,
                    ProductTypeId = 5,
                    YearOfManufacture = 2018,
                },
                new Product
                {
                    Name = "Bibi",
                    Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Price = 620,
                    PriceWithDiscount = 620,
                    FragranceGroupId = 6,
                    ProductTypeId = 6,
                    YearOfManufacture = 2019,
                },
                new Product
                {
                    Name = "Hihi",
                    Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Price = 720,
                    PriceWithDiscount = 720,
                    FragranceGroupId = 7,
                    ProductTypeId = 7,
                    YearOfManufacture = 2020,
                });
            await dbContext.SaveChangesAsync();

            var productsRepository = new EfDeletableEntityRepository<Product>(dbContext);
            var mockRepo = new Mock<IDeletableEntityRepository<Product>>();

            mockRepo.Setup(pv => pv.AllAsNoTracking())
                    .Returns(productsRepository.AllAsNoTracking());

            var service = new ProductsService(mockRepo.Object, null);

            var query = productsRepository.AllAsNoTracking();
            var page = 1;
            var itemsPerPage = 6;
            var allProducts = await service.GetAllProductsAsync(query, string.Empty, "all-products", page, itemsPerPage, (page - 1) * itemsPerPage);

            Assert.Equal(6, await dbContext.Products.CountAsync());
            Assert.Equal(6, allProducts.ItemsCount);
        }

        [Fact]
        public async Task EditProductWithValidModelWorksCorrectly()
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
            await dbContext.SaveChangesAsync();

            var fragranceGroup = new FragranceGroup
            {
                Name = "Chypre",
            };

            await dbContext.FragranceGroups.AddAsync(fragranceGroup);
            await dbContext.SaveChangesAsync();

            var newProduct = new Product
            {
                Name = "Kiki",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Picture = "https://res.cloudinary.com/pictures-storage/image/upload/v1612213773/product_images/y6mh1xtdt7lkmgvrt3gy.jpg",
                Price = 320,
                PriceWithDiscount = 320,
                FragranceGroupId = 2,
                ProductTypeId = 2,
                YearOfManufacture = 2015,
            };

            await dbContext.Products.AddAsync(newProduct);

            var productsRepository = new EfDeletableEntityRepository<Product>(dbContext);
            var mockRepo = new Mock<IDeletableEntityRepository<Product>>();
            var mockNotesService = new Mock<INotesService>();

            mockRepo.Setup(pv => pv.AllAsNoTracking())
                    .Returns(productsRepository.All());
            mockRepo.Setup(pv => pv.SaveChangesAsync())
                    .Returns(productsRepository.SaveChangesAsync());
            mockRepo.Setup(pv => pv.Update(It.IsAny<Product>()))
                    .Callback((Product product) => productsRepository.Update(product));

            var service = new ProductsService(mockRepo.Object, mockNotesService.Object);

            using var stream = File.OpenRead(@"C:\Users\Boris\Downloads\halle-bopp.jpg");
            var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg",
            };

            var editedProduct = new EditProductInputModel
            {
                Name = "Bibi",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Picture = file,
                Price = 320,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2015,
                NoteIds = new string[] { "1" },
            };

            // Act
            var result = await service.EditProductAsync(editedProduct, newProduct.Id, newProduct.Picture);
            var product = await service.GetProductByIdAsync(newProduct.Id);

            // Assert
            Assert.True(result);
            Assert.Equal("Bibi", product.Name);
            Assert.Equal(320, product.Price);
            Assert.Equal(320, product.PriceWithDiscount);
            Assert.Equal(1, product.FragranceGroupId);
            Assert.Equal(1, product.ProductTypeId);
            Assert.Equal(2015, product.YearOfManufacture);
            Assert.Single(product.Notes);
        }

        [Fact]
        public async Task CreateProductWithValidModelWorksCorrectly()
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

            var productsRepository = new EfDeletableEntityRepository<Product>(dbContext);
            var mockRepo = new Mock<IDeletableEntityRepository<Product>>();
            var mockCloudinaryService = new Mock<ICloudinaryService>();
            var mockNotesService = new Mock<INotesService>();

            mockRepo.Setup(pv => pv.AllAsNoTracking())
                    .Returns(productsRepository.All());
            mockRepo.Setup(pv => pv.SaveChangesAsync())
                    .Returns(productsRepository.SaveChangesAsync());
            mockRepo.Setup(pv => pv.AddAsync(It.IsAny<Product>()))
                    .Callback((Product product) => productsRepository.AddAsync(product));

            var service = new ProductsService(mockRepo.Object, mockNotesService.Object);

            var inputModel = new CreateProductInputModel
            {
                Name = "Bibi",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Price = 320,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2015,
                NoteIds = new string[] { "1", "2", "3" },
            };

            // Act
            var result = await service.CreateProductAsync(inputModel, "https://res.cloudinary.com/pictures-storage/image/upload/v1612213773/product_images/y6mh1xtdt7lkmgvrt3gy.jpg");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteProductSuccessfully()
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
            await dbContext.SaveChangesAsync();

            var fragranceGroup = new FragranceGroup
            {
                Name = "Chypre",
            };

            await dbContext.FragranceGroups.AddAsync(fragranceGroup);
            await dbContext.SaveChangesAsync();

            var newProduct = new Product
            {
                Name = "Kiki",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Picture = "https://res.cloudinary.com/pictures-storage/image/upload/v1612213773/product_images/y6mh1xtdt7lkmgvrt3gy.jpg",
                Price = 320,
                PriceWithDiscount = 320,
                FragranceGroupId = 2,
                ProductTypeId = 2,
                YearOfManufacture = 2015,
            };

            await dbContext.Products.AddAsync(newProduct);

            var productsRepository = new EfDeletableEntityRepository<Product>(dbContext);
            var mockRepo = new Mock<IDeletableEntityRepository<Product>>();
            var mockNotesService = new Mock<INotesService>();

            mockRepo.Setup(pv => pv.All())
                    .Returns(productsRepository.All());
            mockRepo.Setup(pv => pv.SaveChangesAsync())
                    .Returns(productsRepository.SaveChangesAsync());
            mockRepo.Setup(pv => pv.Delete(It.IsAny<Product>()))
                    .Callback((Product product) => productsRepository.Delete(product));

            var service = new ProductsService(mockRepo.Object, mockNotesService.Object);

            // Act
            var result = await service.DeleteProductAsync(1);

            // Assert
            Assert.True(result);
            Assert.Equal(0, await dbContext.Products.Where(p => p.DeletedOn != null).CountAsync());
        }

        [Fact]
        public async Task DeleteProductWhenProductsDbContextIsEmptyTheResultShouldBeFalse()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var productsRepository = new EfDeletableEntityRepository<Product>(dbContext);
            var mockRepo = new Mock<IDeletableEntityRepository<Product>>();
            var mockNotesService = new Mock<INotesService>();

            mockRepo.Setup(pv => pv.All())
                    .Returns(productsRepository.All());
            mockRepo.Setup(pv => pv.SaveChangesAsync())
                    .Returns(productsRepository.SaveChangesAsync());
            mockRepo.Setup(pv => pv.Delete(It.IsAny<Product>()))
                    .Callback((Product product) => productsRepository.Delete(product));

            var service = new ProductsService(mockRepo.Object, mockNotesService.Object);

            // Act
            var result = await service.DeleteProductAsync(1);

            // Assert
            Assert.False(result);
            Assert.Equal(0, await dbContext.Products.CountAsync());
        }

        [Fact]
        public async Task UpdateThePricesOfAllProductsAfterTheDiscountIsAppliedSuccessfullyTheResultShouldBeTrue()
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
                PriceWithDiscount = 320,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2015,
            };

            await dbContext.Products.AddAsync(newProduct);

            var productsRepository = new EfDeletableEntityRepository<Product>(dbContext);
            var mockRepo = new Mock<IDeletableEntityRepository<Product>>();
            var mockNotesService = new Mock<INotesService>();

            mockRepo.Setup(pv => pv.All())
                    .Returns(productsRepository.All());
            mockRepo.Setup(pv => pv.AllAsNoTracking())
                    .Returns(productsRepository.AllAsNoTracking());
            mockRepo.Setup(pv => pv.SaveChangesAsync())
                    .Returns(productsRepository.SaveChangesAsync());
            mockRepo.Setup(pv => pv.AddAsync(It.IsAny<Product>()))
                    .Callback((Product product) => productsRepository.AddAsync(product));

            var service = new ProductsService(mockRepo.Object, mockNotesService.Object);

            // Act
            var result = await service.UpdateThePricesOfAllProductsAfterTheDiscountIsAppliedAsync(20);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateThePricesOfAllProductsAfterTheDiscountIsDisabledSuccessfullyTheResultShouldBeTrue()
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
                PriceWithDiscount = 320,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2015,
            };

            await dbContext.Products.AddAsync(newProduct);

            var productsRepository = new EfDeletableEntityRepository<Product>(dbContext);
            var mockRepo = new Mock<IDeletableEntityRepository<Product>>();
            var mockNotesService = new Mock<INotesService>();

            mockRepo.Setup(pv => pv.All())
                    .Returns(productsRepository.All());
            mockRepo.Setup(pv => pv.AllAsNoTracking())
                    .Returns(productsRepository.AllAsNoTracking());
            mockRepo.Setup(pv => pv.SaveChangesAsync())
                    .Returns(productsRepository.SaveChangesAsync());
            mockRepo.Setup(pv => pv.AddAsync(It.IsAny<Product>()))
                    .Callback((Product product) => productsRepository.AddAsync(product));

            var service = new ProductsService(mockRepo.Object, mockNotesService.Object);

            // Act
            var result = await service.UpdateThePricesOfAllProductsAfterTheDiscountIsDisabledAsync();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetProductDetailsWithValidProductId()
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
                PriceWithDiscount = 320,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2015,
            };

            await dbContext.Products.AddAsync(newProduct);

            var productsRepository = new EfDeletableEntityRepository<Product>(dbContext);
            var mockRepo = new Mock<IDeletableEntityRepository<Product>>();

            mockRepo.Setup(pv => pv.All())
                    .Returns(productsRepository.All());
            mockRepo.Setup(pv => pv.AllAsNoTracking())
                    .Returns(productsRepository.AllAsNoTracking());
            mockRepo.Setup(pv => pv.SaveChangesAsync())
                    .Returns(productsRepository.SaveChangesAsync());
            mockRepo.Setup(pv => pv.AddAsync(It.IsAny<Product>()))
                    .Callback((Product product) => productsRepository.AddAsync(product));

            var service = new ProductsService(mockRepo.Object, null);

            // Act
            var product = await service.GetProductByIdAsync(1);

            // Assert
            Assert.Equal("Kiki", product.Name);
            Assert.Equal("https://res.cloudinary.com/pictures-storage/image/upload/v1612213773/product_images/y6mh1xtdt7lkmgvrt3gy.jpg", product.Picture);
            Assert.Equal(320, product.Price);
            Assert.Equal(320, product.PriceWithDiscount);
            Assert.Equal(1, product.FragranceGroupId);
            Assert.Equal(1, product.ProductTypeId);
            Assert.Equal(2015, product.YearOfManufacture);
        }

        [Fact]
        public async Task GetProductDetailsWithInValidProductIdTheResultShouldBeNull()
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
                PriceWithDiscount = 320,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2015,
            };

            await dbContext.Products.AddAsync(newProduct);

            var productsRepository = new EfDeletableEntityRepository<Product>(dbContext);
            var mockRepo = new Mock<IDeletableEntityRepository<Product>>();

            mockRepo.Setup(pv => pv.All())
                    .Returns(productsRepository.All());
            mockRepo.Setup(pv => pv.AllAsNoTracking())
                    .Returns(productsRepository.AllAsNoTracking());
            mockRepo.Setup(pv => pv.SaveChangesAsync())
                    .Returns(productsRepository.SaveChangesAsync());
            mockRepo.Setup(pv => pv.AddAsync(It.IsAny<Product>()))
                    .Callback((Product product) => productsRepository.AddAsync(product));

            var service = new ProductsService(mockRepo.Object, null);

            // Act
            var product = await service.GetProductByIdAsync(2);

            // Assert
            Assert.Null(product);
        }
    }
}
