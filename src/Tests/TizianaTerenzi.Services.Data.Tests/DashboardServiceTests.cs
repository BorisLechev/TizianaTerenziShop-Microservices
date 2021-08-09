namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Data.Dashboard;
    using Xunit;

    public class DashboardServiceTests
    {
        [Fact]
        public async Task GetDashboardInformationSuccessfully()
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

            var role = new ApplicationRole
            {
                Name = "Administrator",
            };

            await dbContext.Roles.AddAsync(role);
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

            var service = new DashboardService(
                new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                new EfDeletableEntityRepository<ApplicationRole>(dbContext),
                new EfDeletableEntityRepository<Order>(dbContext),
                new EfDeletableEntityRepository<OrderProduct>(dbContext));

            // Act
            var dashboard = await service.GetDashboardInformationAsync();

            // Assert
            Assert.Equal(1, dashboard.TotalUsersCount);
            Assert.Equal(0, dashboard.TotalUsersInAdminRoleCount);
            Assert.Equal(0, dashboard.TotalBannedUsersCount);
            Assert.Equal(2, dashboard.TotalOrdersCountForTheCurrentMonth);
            Assert.Equal(3, dashboard.TotalOrderedProductsCountForTheCurrentMonth);
            Assert.Equal(1160, dashboard.TotalRevenueForTheCurrentMonth);
            Assert.Equal(1160, dashboard.TotalRevenueForTheCurrentYear);
        }
    }
}
