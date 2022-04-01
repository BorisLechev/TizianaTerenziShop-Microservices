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
    using TizianaTerenzi.Services.Data.Votes;
    using Xunit;

    public class ProductVotesServiceTests
    {
        [Fact]
        public async Task WhenUserVotes2TimesOnly1VoteShouldBeCounted()
        {
            // Arrange
            var list = new List<ProductVote>();
            var mockList = list.BuildMock();

            var mockRepo = new Mock<IDeletableEntityRepository<ProductVote>>();
            mockRepo.Setup(pv => pv.All())
                    .Returns(mockList);
            mockRepo.Setup(pv => pv.AddAsync(It.IsAny<ProductVote>()))
                    .Callback((ProductVote vote) => list.Add(vote));

            var service = new ProductVotesService(mockRepo.Object);

            // Act
            await service.VoteAsync(1, "1", 1);
            await service.VoteAsync(1, "1", 5);
            await service.VoteAsync(1, "1", 5);
            await service.VoteAsync(1, "1", 5);
            await service.VoteAsync(1, "1", 5);

            // how many times the method AddAsync is used
            mockRepo.Verify(x => x.AddAsync(It.IsAny<ProductVote>()), Times.Exactly(1));

            // Assert
            Assert.Single(list);
            Assert.Equal(5, list.First().Value);
        }

        [Fact]
        public async Task When2UsersVoteForTheSameProductTheAverageVoteShouldBeCorrect()
        {
            // Arrange
            var list = new List<ProductVote>();
            var mockList = list.BuildMock();

            var mockRepo = new Mock<IDeletableEntityRepository<ProductVote>>();
            mockRepo.Setup(pv => pv.All())
                    .Returns(mockList);
            mockRepo.Setup(pv => pv.AllAsNoTracking())
                    .Returns(mockList);
            mockRepo.Setup(pv => pv.AddAsync(It.IsAny<ProductVote>()))
                    .Callback((ProductVote vote) => list.Add(vote));

            var service = new ProductVotesService(mockRepo.Object);

            // Act
            await service.VoteAsync(1, "1", 5);
            await service.VoteAsync(1, "2", 1);
            await service.VoteAsync(1, "1", 4);

            // how many times the method AddAsync is used
            mockRepo.Verify(x => x.AddAsync(It.IsAny<ProductVote>()), Times.Exactly(2));
            var productOneResult = await service.GetNumberOfVotesForEachValueAsync(1);

            // Assert
            Assert.Equal(2, list.Count);
            Assert.Equal(2, productOneResult.CountOfVotes);
            Assert.Equal(2.5, productOneResult.AverageVotes);
        }

        [Fact]
        public async Task VotesMustBeDeletedWhenDeletingTheProduct()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            var service = new ProductVotesService(new EfDeletableEntityRepository<ProductVote>(dbContext));

            // Act
            await service.VoteAsync(1, "1", 5);
            await service.VoteAsync(2, "1", 2);

            await service.DeleteProductVotesAsync(1);

            var productOneResult = await service.GetNumberOfVotesForEachValueAsync(1);
            var productTwoResult = await service.GetNumberOfVotesForEachValueAsync(2);
            var productThreeResult = await service.GetNumberOfVotesForEachValueAsync(3);

            // Assert
            Assert.Null(productOneResult);
            Assert.Equal(1, productTwoResult.CountOfVotes);
            Assert.Null(productThreeResult);
        }

        [Fact]
        public async Task GetAllValuesByProductIdShouldReturnAllValues()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            var service = new ProductVotesService(new EfDeletableEntityRepository<ProductVote>(dbContext));

            // Act
            await service.VoteAsync(1, "1", 5);
            await service.VoteAsync(1, "2", 2);
            await service.VoteAsync(1, "2", 4);

            //var allValues = await service.GetAllValuesByProductIdAsync(1);
            //var averageVote = await service.GetAverageVotesAsync(1);
            var allValues = await service.GetNumberOfVotesForEachValueAsync(1);

            // Assert
            Assert.Equal(2, allValues.CountOfVotes);
            Assert.Equal(4.5, allValues.AverageVotes);
        }
    }
}
