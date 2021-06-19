namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Data.Orders;
    using Xunit;

    public class OrdersServiceTests
    {
        [Fact]
        public async Task GetAllOrdersShouldReturnOneOrder()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var orderStatus = new OrderStatus
            {
                Name = "Status test",
            };
            await dbContext.OrderStatuses.AddAsync(orderStatus);
            await dbContext.SaveChangesAsync();

            var product = new Product
            {
                Name = "Bibi",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Price = 320,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2015,
            };
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            var orderProduct = new OrderProduct
            {
                ProductId = 1,
                Quantity = 1,
                Price = product.Price,
                OrderId = 1,
            };
            await dbContext.OrderProducts.AddAsync(orderProduct);
            await dbContext.SaveChangesAsync();

            var order = new Order
            {
                StatusId = 1,
                UserId = "1",
                Status = orderStatus,
                Products = new List<OrderProduct>() { orderProduct },
            };
            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();

            var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);
            var mockRepo = new Mock<IDeletableEntityRepository<Order>>();

            mockRepo.Setup(n => n.AllAsNoTracking())
                    .Returns(ordersRepository.AllAsNoTracking());

            var service = new OrdersService(mockRepo.Object, null, null);

            var orders = await service.GetAllOrdersAsync();
            var resultOrderProduct = orders.First().Products.First();

            Assert.Single(orders);
            Assert.Equal(product.Price, resultOrderProduct.Price);
            Assert.Equal(orderProduct.Quantity, resultOrderProduct.Quantity);
        }

        [Fact]
        public async Task GetAllOrdersByUserIdShouldReturnOneOrder()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var orderStatus = new OrderStatus
            {
                Name = "Status test",
            };
            await dbContext.OrderStatuses.AddAsync(orderStatus);
            await dbContext.SaveChangesAsync();

            var product = new Product
            {
                Name = "Bibi",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Price = 320,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2015,
            };
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            var orderProducts = new List<OrderProduct>
            {
                new OrderProduct
                {
                    ProductId = 1,
                    Quantity = 1,
                    Price = 250,
                    OrderId = 1,
                },
                new OrderProduct
                {
                    ProductId = 2,
                    Quantity = 2,
                    Price = 260,
                    OrderId = 1,
                },
            };

            await dbContext.OrderProducts.AddRangeAsync(orderProducts);
            await dbContext.SaveChangesAsync();

            var order = new Order
            {
                StatusId = 1,
                UserId = "1",
                Status = orderStatus,
                Products = orderProducts,
            };
            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();

            var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);
            var mockRepo = new Mock<IDeletableEntityRepository<Order>>();

            mockRepo.Setup(n => n.AllAsNoTracking())
                    .Returns(ordersRepository.AllAsNoTracking());

            var service = new OrdersService(mockRepo.Object, null, null);

            // Act
            var orders = await service.GetAllOrdersByUserIdAsync("1");
            var resultOrderProduct1 = orders.First().Products.First(p => p.Price == 250);
            var resultOrderProduct2 = orders.First().Products.First(p => p.Price == 260);

            // Assert
            Assert.Single(orders);
            Assert.Equal(250, resultOrderProduct1.Price);
            Assert.Equal(1, resultOrderProduct1.Quantity);
            Assert.Equal(260, resultOrderProduct2.Price);
            Assert.Equal(2, resultOrderProduct2.Quantity);
        }

        [Fact]
        public async Task GetAllPendingOrdersShouldReturnOneOrder()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var orderStatus = new OrderStatus
            {
                Name = "Pending",
            };
            await dbContext.OrderStatuses.AddAsync(orderStatus);
            await dbContext.SaveChangesAsync();

            var product = new Product
            {
                Name = "Bibi",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Price = 320,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2015,
            };
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            var orderProduct = new OrderProduct
            {
                ProductId = 1,
                Quantity = 1,
                Price = product.Price,
                OrderId = 1,
            };
            await dbContext.OrderProducts.AddAsync(orderProduct);
            await dbContext.SaveChangesAsync();

            var order = new Order
            {
                StatusId = 1,
                UserId = "1",
                Status = orderStatus,
                Products = new List<OrderProduct>() { orderProduct },
            };
            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();

            var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);
            var mockRepo = new Mock<IDeletableEntityRepository<Order>>();

            mockRepo.Setup(n => n.AllAsNoTracking())
                    .Returns(ordersRepository.AllAsNoTracking());

            var service = new OrdersService(mockRepo.Object, null, null);

            // Act
            var orders = await service.GetAllPendingOrdersAsync();
            var resultOrder = orders.First();
            var resultOrderProduct = orders.First().Products.First();

            // Assert
            Assert.Single(orders);
            Assert.Equal(product.Price, resultOrderProduct.Price);
            Assert.Equal(orderProduct.Quantity, resultOrderProduct.Quantity);
            Assert.Equal(orderStatus.Name, resultOrder.StatusName);
        }

        [Fact]
        public async Task GetAllProcessedOrdersShouldReturnOneOrder()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            await dbContext.OrderStatuses.AddRangeAsync(new List<OrderStatus>
            {
                new OrderStatus
                {
                    Name = "Completed",
                },
                new OrderStatus
                {
                    Name = "Pending",
                },
            });
            await dbContext.SaveChangesAsync();

            var product = new Product
            {
                Name = "Bibi",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Price = 320,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2015,
            };
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            var orderProductOne = new OrderProduct
            {
                ProductId = 1,
                Quantity = 1,
                Price = product.Price,
                OrderId = 1,
            };
            var orderProductTwo = new OrderProduct
            {
                ProductId = 1,
                Quantity = 2,
                Price = 350,
                OrderId = 2,
            };
            await dbContext.OrderProducts.AddRangeAsync(new List<OrderProduct> { orderProductOne, orderProductTwo });
            await dbContext.SaveChangesAsync();

            await dbContext.Orders.AddRangeAsync(new List<Order>
            {
                new Order
                {
                    StatusId = 1,
                    UserId = "1",
                    Products = new List<OrderProduct>() { orderProductOne },
                },
                new Order
                {
                    StatusId = 2,
                    UserId = "1",
                    Products = new List<OrderProduct>() { orderProductTwo },
                },
            });
            await dbContext.SaveChangesAsync();

            var ordersRepository = new EfDeletableEntityRepository<Order>(dbContext);
            var mockRepo = new Mock<IDeletableEntityRepository<Order>>();

            mockRepo.Setup(n => n.AllAsNoTracking())
                    .Returns(ordersRepository.AllAsNoTracking());

            var service = new OrdersService(mockRepo.Object, null, null);

            // Act
            var orders = await service.GetAllProcessedOrdersAsync();
            var resultOrder = orders.First();
            var resultOrderProduct = orders.First().Products.First();

            // Assert
            Assert.Single(orders);
            Assert.Equal(product.Price, resultOrderProduct.Price);
            Assert.Equal(1, resultOrderProduct.Quantity);
            Assert.Equal("Completed", resultOrder.StatusName);
        }

        [Fact]
        public async Task GetAllOrderProductsByOrderIdShouldReturnOneOrder()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            await dbContext.OrderStatuses.AddRangeAsync(new List<OrderStatus>
            {
                new OrderStatus
                {
                    Name = "Completed",
                },
                new OrderStatus
                {
                    Name = "Pending",
                },
            });
            await dbContext.SaveChangesAsync();

            var product1 = new Product
            {
                Name = "Bibi",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Price = 320,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2015,
            };

            var product2 = new Product
            {
                Name = "Zizi",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Price = 120,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2017,
            };

            var product3 = new Product
            {
                Name = "Fifi",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Price = 720,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2018,
            };

            await dbContext.Products.AddRangeAsync(new List<Product>
            {
                product1,
                product2,
                product3,
            });
            await dbContext.SaveChangesAsync();

            var orderProductOne = new OrderProduct
            {
                ProductId = 1,
                Quantity = 1,
                Price = product1.Price,
                OrderId = 1,
            };
            var orderProductTwo = new OrderProduct
            {
                ProductId = 2,
                Quantity = 2,
                Price = product2.Price,
                OrderId = 2,
            };
            var orderProductThree = new OrderProduct
            {
                ProductId = 3,
                Quantity = 5,
                Price = product3.Price,
                OrderId = 1,
            };

            await dbContext.OrderProducts.AddRangeAsync(new List<OrderProduct>
            {
                orderProductOne,
                orderProductTwo,
                orderProductThree,
            });
            await dbContext.SaveChangesAsync();

            await dbContext.Orders.AddRangeAsync(new List<Order>
            {
                new Order
                {
                    StatusId = 1,
                    UserId = "1",
                    Products = new List<OrderProduct>() { orderProductTwo, orderProductThree },
                },
                new Order
                {
                    StatusId = 2,
                    UserId = "1",
                    Products = new List<OrderProduct>() { orderProductOne },
                },
            });
            await dbContext.SaveChangesAsync();

            var mockOrderStatusesService = new Mock<IOrderStatusesService>();

            var service = new OrdersService(
                new EfDeletableEntityRepository<Order>(dbContext),
                new EfDeletableEntityRepository<OrderProduct>(dbContext),
                mockOrderStatusesService.Object);

            // Act
            var result = await service.ProcessOrderAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetAllOrderProductsByOrderIdTheResultShouldBeArrayOfOrderProducts()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            await dbContext.OrderStatuses.AddRangeAsync(new List<OrderStatus>
            {
                new OrderStatus
                {
                    Name = "Completed",
                },
                new OrderStatus
                {
                    Name = "Pending",
                },
            });
            await dbContext.SaveChangesAsync();

            var product1 = new Product
            {
                Name = "Bibi",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Price = 320,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2015,
            };

            var product2 = new Product
            {
                Name = "Zizi",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Price = 120,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2017,
            };

            var product3 = new Product
            {
                Name = "Fifi",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Price = 720,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2018,
            };

            await dbContext.Products.AddRangeAsync(new List<Product>
            {
                product1,
                product2,
                product3,
            });
            await dbContext.SaveChangesAsync();

            var orderProductOne = new OrderProduct
            {
                ProductId = 1,
                Quantity = 1,
                Price = product1.Price,
                OrderId = 1,
            };
            var orderProductTwo = new OrderProduct
            {
                ProductId = 2,
                Quantity = 2,
                Price = product2.Price,
                OrderId = 2,
            };
            var orderProductThree = new OrderProduct
            {
                ProductId = 3,
                Quantity = 5,
                Price = product3.Price,
                OrderId = 1,
            };

            await dbContext.OrderProducts.AddRangeAsync(new List<OrderProduct>
            {
                orderProductOne,
                orderProductTwo,
                orderProductThree,
            });
            await dbContext.SaveChangesAsync();

            await dbContext.Orders.AddRangeAsync(new List<Order>
            {
                new Order
                {
                    StatusId = 1,
                    UserId = "1",
                    Products = new List<OrderProduct>() { orderProductTwo, orderProductThree },
                },
                new Order
                {
                    StatusId = 2,
                    UserId = "1",
                    Products = new List<OrderProduct>() { orderProductOne },
                },
            });
            await dbContext.SaveChangesAsync();

            var mockOrderStatusesService = new Mock<IOrderStatusesService>();

            var service = new OrdersService(
                new EfDeletableEntityRepository<Order>(dbContext),
                new EfDeletableEntityRepository<OrderProduct>(dbContext),
                mockOrderStatusesService.Object);

            // Act
            var orderProducts = await service.GetAllOrderProductsByOrderIdAsync(1);
            var orderProduct2 = orderProducts.First(op => op.ProductName == product2.Name);
            var orderProduct3 = orderProducts.First(op => op.ProductName == product3.Name);

            // Assert
            Assert.Equal(2, orderProducts.Count());
            Assert.Equal(product2.Name, orderProduct2.ProductName);
            Assert.Equal(product2.Price, orderProduct2.Price);
            Assert.Equal(product3.Name, orderProduct3.ProductName);
            Assert.Equal(product3.Price, orderProduct3.Price);
        }

        [Fact]
        public async Task DeleteAllOrdersByUserId()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            await dbContext.OrderStatuses.AddRangeAsync(new List<OrderStatus>
            {
                new OrderStatus
                {
                    Name = "Completed",
                },
                new OrderStatus
                {
                    Name = "Pending",
                },
            });
            await dbContext.SaveChangesAsync();

            var product1 = new Product
            {
                Name = "Bibi",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Price = 320,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2015,
            };

            var product2 = new Product
            {
                Name = "Zizi",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Price = 120,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2017,
            };

            var product3 = new Product
            {
                Name = "Fifi",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Price = 720,
                FragranceGroupId = 1,
                ProductTypeId = 1,
                YearOfManufacture = 2018,
            };

            await dbContext.Products.AddRangeAsync(new List<Product>
            {
                product1,
                product2,
                product3,
            });
            await dbContext.SaveChangesAsync();

            var orderProductOne = new OrderProduct
            {
                ProductId = 1,
                Quantity = 1,
                Price = product1.Price,
                OrderId = 1,
            };
            var orderProductTwo = new OrderProduct
            {
                ProductId = 2,
                Quantity = 2,
                Price = product2.Price,
                OrderId = 2,
            };
            var orderProductThree = new OrderProduct
            {
                ProductId = 3,
                Quantity = 5,
                Price = product3.Price,
                OrderId = 1,
            };

            await dbContext.OrderProducts.AddRangeAsync(new List<OrderProduct>
            {
                orderProductOne,
                orderProductTwo,
                orderProductThree,
            });
            await dbContext.SaveChangesAsync();

            await dbContext.Orders.AddRangeAsync(new List<Order>
            {
                new Order
                {
                    StatusId = 1,
                    UserId = "1",
                    Products = new List<OrderProduct>() { orderProductTwo, orderProductThree },
                },
                new Order
                {
                    StatusId = 2,
                    UserId = "1",
                    Products = new List<OrderProduct>() { orderProductOne },
                },
            });
            await dbContext.SaveChangesAsync();

            var mockOrderStatusesService = new Mock<IOrderStatusesService>();

            var service = new OrdersService(
                new EfDeletableEntityRepository<Order>(dbContext),
                new EfDeletableEntityRepository<OrderProduct>(dbContext),
                mockOrderStatusesService.Object);

            // Act
            var result = await service.DeleteAllOrdersByUserIdAsync("1");

            // Assert
            Assert.True(result);
        }

        // TODO DeleteAllOrderProductsByUserIdAsync Z.EntityFramework.Plus
    }
}
