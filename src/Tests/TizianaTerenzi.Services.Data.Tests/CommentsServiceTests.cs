namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Data.Comments;
    using TizianaTerenzi.WebClient.ViewModels.Comments;
    using Xunit;

    public class CommentsServiceTests
    {
        [Fact]
        public async Task AddCommentSuccessfully()
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

            var comment = new CreateCommentInputModel
            {
                ProductId = 1,
                ParentId = null,
                Content = "Test",
            };

            var service = new CommentsService(
                new EfDeletableEntityRepository<Comment>(dbContext));

            // Act
            var createCommentResult = await service.CreateAsync(comment, user.Id);

            // Assert
            Assert.True(createCommentResult);
        }

        [Fact]
        public async Task IsCommentInProductShouldWorkCorrectly()
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

            var comment = new Comment
            {
                ProductId = 1,
                ParentId = null,
                UserId = user.Id,
                Content = "Test",
            };

            await dbContext.Comments.AddAsync(comment);
            await dbContext.SaveChangesAsync();

            var service = new CommentsService(
                new EfDeletableEntityRepository<Comment>(dbContext));

            // Act
            var isCommentInProduct = await service.IsInProductIdAsync(comment.Id, 1);
            var isCommentInProductFalse = await service.IsInProductIdAsync(comment.Id, 2);

            // Assert
            Assert.True(isCommentInProduct);
            Assert.False(isCommentInProductFalse);
        }

        [Fact]
        public async Task DeleteRangeByProductIdShouldWorkCorrectly()
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

            var comment = new Comment
            {
                ProductId = 1,
                ParentId = null,
                UserId = user.Id,
                Content = "Test",
            };

            await dbContext.Comments.AddAsync(comment);
            await dbContext.SaveChangesAsync();

            var service = new CommentsService(
                new EfDeletableEntityRepository<Comment>(dbContext));

            // Act
            var resultTrue = await service.DeleteRangeByProductIdAsync(1);
            var resultSecondTrue = await service.DeleteRangeByProductIdAsync(2);

            // Assert
            Assert.True(resultTrue);
            Assert.True(resultSecondTrue);
        }
    }
}
