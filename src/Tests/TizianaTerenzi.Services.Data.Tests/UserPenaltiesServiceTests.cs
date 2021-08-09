namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Data.UserPenalties;
    using Xunit;

    public class UserPenaltiesServiceTests
    {
        [Fact]
        public async Task BlockUserSuccessfully()
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

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            userManager
                .Setup(x => x.UpdateSecurityStampAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            var service = new UserPenaltiesService(
                new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                userManager.Object);

            // Act
            var firstBlockResult = await service.BlockUserAsync(user.Id, "losh chovek");
            var allBlockedUsers = await service.GetAllBlockedUsersAsync();
            var secondBlockResult = await service.BlockUserAsync(user.Id, "bla-bla");

            // Assert
            Assert.True(firstBlockResult);
            Assert.Single(allBlockedUsers);
            Assert.Equal(user.UserName, allBlockedUsers.First().Text);
            Assert.Equal(user.Id, allBlockedUsers.First().Value);
            Assert.False(secondBlockResult);
        }

        [Fact]
        public async Task UnblockUserSuccessfully()
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

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            userManager
                .Setup(x => x.UpdateSecurityStampAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            var service = new UserPenaltiesService(
                new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                userManager.Object);

            // Act
            var firstBlockResult = await service.BlockUserAsync(user.Id, "losh chovek");
            var allBlockedUsers = await service.GetAllBlockedUsersAsync();
            var secondBlockResult = await service.BlockUserAsync(user.Id, "bla-bla");

            // Assert
            Assert.True(firstBlockResult);
            Assert.False(secondBlockResult);
            Assert.Single(allBlockedUsers);
            Assert.Equal(user.UserName, allBlockedUsers.First().Text);
            Assert.Equal(user.Id, allBlockedUsers.First().Value);

            var firstUnblockResult = await service.UnblockUserAsync(user.Id);
            var secondUnblockResult = await service.UnblockUserAsync(user.Id);
            var allUnblockedUsers = await service.GetAllUnblockedUsersAsync();

            Assert.True(firstUnblockResult);
            Assert.False(secondUnblockResult);
            Assert.Single(allUnblockedUsers);
        }
    }
}
