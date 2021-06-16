namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Data.Orders;
    using Xunit;

    public class OrderStatusesServiceTests
    {
        [Fact]
        public async Task FindOrderStatusByNameTheResultShouldBe1()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var orderStatus = new OrderStatus
            {
                Name = "Test",
            };

            await dbContext.OrderStatuses.AddAsync(orderStatus);
            await dbContext.SaveChangesAsync();

            var orderStatusesRepository = new EfDeletableEntityRepository<OrderStatus>(dbContext);
            var mockRepo = new Mock<IDeletableEntityRepository<OrderStatus>>();

            mockRepo.Setup(n => n.AllAsNoTracking())
                    .Returns(orderStatusesRepository.AllAsNoTracking());

            var service = new OrderStatusesService(mockRepo.Object);

            var orderStatusId = await service.FindByNameAsync(orderStatus.Name);

            Assert.Equal(1, orderStatusId);
        }

        [Fact]
        public async Task FindOrderStatusByNameTheResultShouldBe0()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var orderStatus = new OrderStatus
            {
                Name = "Test",
            };

            await dbContext.OrderStatuses.AddAsync(orderStatus);
            await dbContext.SaveChangesAsync();

            var orderStatusesRepository = new EfDeletableEntityRepository<OrderStatus>(dbContext);
            var mockRepo = new Mock<IDeletableEntityRepository<OrderStatus>>();

            mockRepo.Setup(n => n.AllAsNoTracking())
                    .Returns(orderStatusesRepository.AllAsNoTracking());

            var service = new OrderStatusesService(mockRepo.Object);

            var orderStatusId = await service.FindByNameAsync("test");

            Assert.Equal(0, orderStatusId);
        }
    }
}
