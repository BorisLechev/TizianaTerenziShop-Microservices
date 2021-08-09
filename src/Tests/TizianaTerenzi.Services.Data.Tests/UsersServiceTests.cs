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
    using TizianaTerenzi.Services.Data.Users;
    using Xunit;

    public class UsersServiceTests
    {
        [Fact]
        public async Task GetAllBannedUsersSuccessfully()
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

            var userPenaltiesService = new UserPenaltiesService(
                new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                userManager.Object);

            var usersService = new UsersService(
                new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                new EfDeletableEntityRepository<ApplicationRole>(dbContext),
                null,
                null);

            // Act
            var firstBlockResult = await userPenaltiesService.BlockUserAsync(user.Id, "losh chovek");
            var allBannedUsers = await usersService.GetAllBannedUsersAsync();

            // Assert
            Assert.True(firstBlockResult);
            Assert.Single(allBannedUsers);
        }

        [Fact]
        public async Task GetUsernamesRolesSuccessfully()
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

            var usersService = new UsersService(
                new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                new EfDeletableEntityRepository<ApplicationRole>(dbContext),
                null,
                null);

            // Act
            var usernamesRoles = await usersService.GetUsernamesRolesAsync();
            var roles = usernamesRoles.Roles;
            var usernames = usernamesRoles.Users;

            // Assert
            Assert.Single(roles);
            Assert.Single(usernames);
            Assert.Equal(role.Name, roles.First().Text);
            Assert.Equal(role.Id, roles.First().Value);
            Assert.Equal(user.UserName, usernames.First().Text);
            Assert.Equal(user.Id, usernames.First().Value);
        }

        //[Fact]
        //public async Task IsUserInRoleSuccessfully()
        //{
        //    // Arrange
        //    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString());
        //    var dbContext = new ApplicationDbContext(optionsBuilder.Options);

        //    var user = new ApplicationUser
        //    {
        //        FirstName = "FirstName2",
        //        LastName = "LastName2",
        //        UserName = "mail2@example.com",
        //        Email = "mail1@example.com",
        //        Town = "Test",
        //        PostalCode = "1000",
        //        Address = "Test",
        //    };

        //    await dbContext.Users.AddAsync(user);
        //    await dbContext.SaveChangesAsync();

        //    var role = new ApplicationRole
        //    {
        //        Name = "Administrator",
        //    };

        //    await dbContext.Roles.AddAsync(role);
        //    await dbContext.SaveChangesAsync();

        //    var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        //    var userManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

        //    var roleStoreMock = new Mock<IUserStore<ApplicationRole>>();
        //    var roleManager = new Mock<RoleManager<ApplicationRole>>(roleStoreMock.Object);

        //    userManager
        //        .Setup(x => x.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
        //        .ReturnsAsync(It.IsAny<bool>());

        //    roleManager
        //        .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
        //        .ReturnsAsync(It.IsAny<ApplicationRole>());

        //    var usersService = new UsersService(
        //        new EfDeletableEntityRepository<ApplicationUser>(dbContext),
        //        new EfDeletableEntityRepository<ApplicationRole>(dbContext),
        //        userManager.Object,
        //        roleManager.Object);

        //    // Act
        //    var result = await usersService.IsUserAlreadyAddedInRoleAsync(user.Id, role.Id);

        //    // Assert
        //    Assert.False(result);
        //}
    }
}
