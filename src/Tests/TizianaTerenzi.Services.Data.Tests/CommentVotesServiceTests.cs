namespace TizianaTerenzi.Services.Data.Tests
{
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

    public class CommentVotesServiceTests
    {
        [Fact]
        public async Task WhenUserVoteForTheSameCommentAndTheFirstVoteTypeIsUpVoteThenUserVoteShouldNotBeCounted()
        {
            // Arrange
            var list = new List<CommentVote>();
            var mockList = list.AsQueryable().BuildMock();

            var mockRepo = new Mock<IDeletableEntityRepository<CommentVote>>();
            mockRepo.Setup(cv => cv.All())
                    .Returns(mockList.Object);
            mockRepo.Setup(cv => cv.AddAsync(It.IsAny<CommentVote>()))
                    .Callback((CommentVote vote) => list.Add(vote));

            var service = new CommentVotesService(mockRepo.Object, null);

            // Act
            await service.VoteAsync(1, "1");
            await service.VoteAsync(1, "1");

            // how many times the method AddAsync is used
            mockRepo.Verify(x => x.AddAsync(It.IsAny<CommentVote>()), Times.Exactly(1));

            // Assert
            Assert.Equal(0, await service.GetVotesAsync(1));
        }

        [Fact]
        public async Task WhenUserVoteForTheSameCommentAndTheFirstVoteTypeIsNeutralThenUserVoteShouldBeCounted()
        {
            // Arrange
            var list = new List<CommentVote>();
            var mockList = list.AsQueryable().BuildMock();

            var mockRepo = new Mock<IDeletableEntityRepository<CommentVote>>();
            mockRepo.Setup(cv => cv.All())
                    .Returns(mockList.Object);
            mockRepo.Setup(cv => cv.AddAsync(It.IsAny<CommentVote>()))
                    .Callback((CommentVote vote) => list.Add(vote));

            var service = new CommentVotesService(mockRepo.Object, null);

            // Act
            await service.VoteAsync(1, "1");
            await service.VoteAsync(1, "1");
            await service.VoteAsync(1, "1");

            // how many times the method AddAsync is used
            mockRepo.Verify(x => x.AddAsync(It.IsAny<CommentVote>()), Times.Exactly(1));

            // Assert
            Assert.Equal(1, await service.GetVotesAsync(1));
        }

        [Fact]
        public async Task WhenSeveralUsersVoteForTheSameCommentTheVotesCountShouldBeCorrect()
        {
            // Arrange
            var list = new List<CommentVote>();
            var mockList = list.AsQueryable().BuildMock();

            var mockRepo = new Mock<IDeletableEntityRepository<CommentVote>>();
            mockRepo.Setup(cv => cv.All())
                    .Returns(mockList.Object);
            mockRepo.Setup(cv => cv.AddAsync(It.IsAny<CommentVote>()))
                    .Callback((CommentVote vote) => list.Add(vote));

            var service = new CommentVotesService(mockRepo.Object, null);

            // Act
            await service.VoteAsync(1, "2");
            await service.VoteAsync(1, "1");
            await service.VoteAsync(1, "3");
            await service.VoteAsync(1, "1");
            await service.VoteAsync(1, "2");
            await service.VoteAsync(1, "2");
            await service.VoteAsync(1, "4");

            // how many times the method AddAsync is used
            mockRepo.Verify(x => x.AddAsync(It.IsAny<CommentVote>()), Times.Exactly(4));

            // Assert
            Assert.Equal(3, await service.GetVotesAsync(1));
        }

        [Fact]
        public async Task VotesMustBeDeletedWhenDeletingTheUser()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            var service = new CommentVotesService(new EfDeletableEntityRepository<CommentVote>(dbContext), null);

            // Act
            await service.VoteAsync(1, "1");
            await service.VoteAsync(2, "3");
            await service.VoteAsync(3, "2");
            await service.DeleteRangeByUserIdAsync("1");
            await service.DeleteRangeByUserIdAsync("2");
            await service.VoteAsync(3, "2");

            // Assert
            Assert.Equal(0, await service.GetVotesAsync(1));
            Assert.Equal(1, await service.GetVotesAsync(2));
            Assert.Equal(1, await service.GetVotesAsync(3));
        }

        [Fact]
        public async Task VotesMustBeDeletedWhenDeletingTheProduct()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            var service = new CommentVotesService(new EfDeletableEntityRepository<CommentVote>(dbContext), new EfDeletableEntityRepository<Comment>(dbContext));

            var comments = new List<Comment>();
            var comment1 = new Comment
            {
                Id = 1,
                ProductId = 1,
                ParentId = 0,
                UserId = "1",
                Content = "aaa",
            };
            comments.Add(comment1);

            var comment2 = new Comment
            {
                Id = 2,
                ProductId = 2,
                ParentId = 0,
                UserId = "2",
                Content = "bbb",
            };
            comments.Add(comment2);

            await dbContext.Comments.AddRangeAsync(comments);
            await dbContext.SaveChangesAsync();

            // Act
            await service.VoteAsync(1, "1");
            await service.VoteAsync(2, "3");
            await service.VoteAsync(2, "3");
            await service.VoteAsync(2, "2");
            await service.DeleteRangeByProductIdAsync(1);

            // Assert
            Assert.Equal(0, await service.GetVotesAsync(1));
            Assert.Equal(2, await service.GetVotesAsync(2));
        }
    }
}
