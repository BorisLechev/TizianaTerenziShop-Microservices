namespace TizianaTerenzi.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MockQueryable.Moq;
    using Moq;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Subscribe;
    using Xunit;

    public class SubscribeServiceTests
    {
        [Fact]
        public async Task WhenUserTriesToSubscribeWith1EmailToANewsletterSeveralTimesTheResultShouldBeTrue()
        {
            // Arrange
            var list = new List<Subscriber>();
            var mockList = list.AsQueryable().BuildMock();

            var mockRepo = new Mock<IDeletableEntityRepository<Subscriber>>();
            mockRepo.Setup(pv => pv.AllAsNoTracking())
                    .Returns(mockList.Object);
            mockRepo.Setup(pv => pv.AddAsync(It.IsAny<Subscriber>()))
                    .Callback((Subscriber subscriber) => list.Add(subscriber));

            var service = new SubscribeService(mockRepo.Object);

            // Act
            await service.SubscribeForNewsletterAsync("a@abv.bg");
            await service.SubscribeForNewsletterAsync("a@abv.bg");

            // how many times the method AddAsync is used
            mockRepo.Verify(x => x.AddAsync(It.IsAny<Subscriber>()), Times.Exactly(1));

            // Assert
            Assert.True(await service.IsTheEmailAlreadySubscribedAsync("a@abv.bg"));
        }

        [Fact]
        public async Task WhenUserTriesToSubscribeWith1EmailToANewsletterSeveralTimesOnly1TimeTheEmailShouldBeSaved()
        {
            // Arrange
            var list = new List<Subscriber>();
            var mockList = list.AsQueryable().BuildMock();

            var mockRepo = new Mock<IDeletableEntityRepository<Subscriber>>();
            mockRepo.Setup(pv => pv.AllAsNoTracking())
                    .Returns(mockList.Object);
            mockRepo.Setup(pv => pv.AddAsync(It.IsAny<Subscriber>()))
                    .Callback((Subscriber subscriber) => list.Add(subscriber));

            var service = new SubscribeService(mockRepo.Object);

            // Act
            await service.SubscribeForNewsletterAsync("a@abv.bg");
            await service.SubscribeForNewsletterAsync("a@abv.bg");
            await service.SubscribeForNewsletterAsync("a@abv.bg");
            await service.SubscribeForNewsletterAsync("a@abv.bg");
            await service.SubscribeForNewsletterAsync("a@abv.bg");
            await service.SubscribeForNewsletterAsync("a@abv.bg");
            await service.SubscribeForNewsletterAsync("b@abv.bg");

            // how many times the method AddAsync is used
            mockRepo.Verify(x => x.AddAsync(It.IsAny<Subscriber>()), Times.Exactly(2));

            // Assert
            Assert.Equal(2, list.Count);
        }
    }
}
