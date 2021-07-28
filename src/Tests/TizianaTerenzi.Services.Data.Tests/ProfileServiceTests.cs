namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Data.Chat;
    using TizianaTerenzi.Services.Data.Comments;
    using TizianaTerenzi.Services.Data.Countries;
    using TizianaTerenzi.Services.Data.Notifications;
    using TizianaTerenzi.Services.Data.Orders;
    using TizianaTerenzi.Services.Data.Profile;
    using TizianaTerenzi.Services.Data.Votes;
    using TizianaTerenzi.Services.Data.Wishlist;
    using TizianaTerenzi.Web.ViewModels.Profile;
    using Xunit;

    public class ProfileServiceTests
    {
        [Fact]
        public async Task DeleteUserWithCorrectIdWorksCorrectly()
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
                Comments = new List<Comment>
                {
                    new Comment
                    {
                        Product = new Product
                        {
                            Name = "Telea",
                            Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                            Price = 320,
                            PriceWithGeneralDiscount = 320,
                            YearOfManufacture = 2015,
                        },
                        Content = "first comment",
                    },
                    new Comment
                    {
                        Product = new Product
                        {
                            Name = "Telea",
                            Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                            Price = 320,
                            PriceWithGeneralDiscount = 320,
                            YearOfManufacture = 2015,
                        },
                        Content = "first comment",
                    },
                },
                FavoriteProducts = new List<FavoriteProduct>
                {
                    new FavoriteProduct
                    {
                        Product = new Product
                        {
                            Name = "Kira",
                            Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                            Price = 420,
                            PriceWithGeneralDiscount = 420,
                            YearOfManufacture = 2016,
                        },
                    },
                    new FavoriteProduct
                    {
                        Product = new Product
                        {
                            Name = "Kira",
                            Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                            Price = 420,
                            PriceWithGeneralDiscount = 420,
                            YearOfManufacture = 2016,
                        },
                    },
                },
                UserNotifications = new List<ApplicationUserNotification>
                {
                    new ApplicationUserNotification
                    {
                        ReceiverUsername = "mail2@example.com",
                        Link = "...",
                        Text = "zdr",
                        SenderId = "1",
                    },
                }
                .ToArray(),
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var mockCountriesService = new Mock<ICountriesService>();
            var mockWishlistsService = new Mock<IWishlistService>();
            var mockOrdersService = new Mock<IOrdersService>();
            var mockCommentsService = new Mock<ICommentsService>();
            var mockCommentVotesService = new Mock<ICommentVotesService>();
            var mockNotificationsService = new Mock<INotificationsService>();

            var chatGroup = new ChatGroup();

            var groupReceiver = new ChatUserGroup
            {
                ChatGroup = chatGroup,
                User = user,
            };

            var groupSender = new ChatUserGroup
            {
                ChatGroup = chatGroup,
                User = user,
            };

            chatGroup.ChatUserGroups.Add(groupReceiver);
            chatGroup.ChatUserGroups.Add(groupSender);

            await dbContext.ChatGroups.AddAsync(chatGroup);
            await dbContext.SaveChangesAsync();

            var profileService = new ProfileService(
                new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                new EfDeletableEntityRepository<ApplicationRole>(dbContext),
                mockCountriesService.Object,
                mockWishlistsService.Object,
                mockOrdersService.Object,
                mockCommentsService.Object,
                mockCommentVotesService.Object,
                mockNotificationsService.Object,
                null);

            var commentsService = new CommentsService(new EfDeletableEntityRepository<Comment>(dbContext));
            var wishlistService = new WishlistService(new EfDeletableEntityRepository<FavoriteProduct>(dbContext));
            var chatsService = new ChatService(
                new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                new EfDeletableEntityRepository<ChatGroup>(dbContext),
                new EfDeletableEntityRepository<ChatMessage>(dbContext));
            var notificationsService = new NotificationsService(
                new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                new EfDeletableEntityRepository<ApplicationUserNotification>(dbContext));

            Assert.Equal(1, await dbContext.Users.CountAsync());
            Assert.Equal(2, await dbContext.Comments.CountAsync());
            Assert.Equal(2, await dbContext.FavoriteProducts.CountAsync());
            Assert.Equal(1, await dbContext.UserNotifications.CountAsync());

            var userResult = await profileService.DeleteUserAsync(user);
            Assert.True(userResult);
            Assert.Equal(0, await dbContext.Users.CountAsync());

            var commentsResult = await commentsService.DeleteRangeByUserIdAsync(user.Id);
            Assert.True(commentsResult);
            Assert.Equal(0, await dbContext.Comments.CountAsync());

            var wishlistResult = await wishlistService.DeleteAllProductsInTheWishlistAsync(user.Id);
            Assert.True(wishlistResult);
            Assert.Equal(0, await dbContext.FavoriteProducts.CountAsync());

            var notificationsResult = await notificationsService.DeleteAllNotificationsByUserIdAsync(user.Id, user.UserName);
            Assert.True(notificationsResult);
            Assert.Equal(0, await dbContext.UserNotifications.CountAsync());
        }

        [Fact]
        public async Task DeleteUserWithNullUserIdReturnsFalse()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var mockCountriesService = new Mock<ICountriesService>();
            var mockWishlistsService = new Mock<IWishlistService>();
            var mockOrdersService = new Mock<IOrdersService>();
            var mockCommentsService = new Mock<ICommentsService>();
            var mockCommentVotesService = new Mock<ICommentVotesService>();

            var personalDataService = new ProfileService(
                new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                new EfDeletableEntityRepository<ApplicationRole>(dbContext),
                mockCountriesService.Object,
                mockWishlistsService.Object,
                mockOrdersService.Object,
                mockCommentsService.Object,
                mockCommentVotesService.Object,
                null,
                null);

            // Act
            var result = await personalDataService.DeleteUserAsync(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetPersonalDataForUserJsonWithCorrectUserIdWorksCorrectly()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var user = new ApplicationUser
            {
                FirstName = "FirstName",
                LastName = "LastName",
                UserName = "aa@example.com",
                Email = "aa@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
                Comments = new List<Comment>
                {
                    new Comment
                    {
                        Product = new Product
                        {
                            Name = "Telea",
                            Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                            Price = 320,
                            PriceWithGeneralDiscount = 320,
                            YearOfManufacture = 2015,
                        },
                        Content = "first comment",
                    },
                    new Comment
                    {
                        Product = new Product
                        {
                            Name = "Telea",
                            Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                            Price = 320,
                            PriceWithGeneralDiscount = 320,
                            YearOfManufacture = 2015,
                        },
                        Content = "first comment",
                    },
                },
                FavoriteProducts = new List<FavoriteProduct>
                {
                    new FavoriteProduct
                    {
                        Product = new Product
                        {
                            Name = "Kira",
                            Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                            Price = 420,
                            PriceWithGeneralDiscount = 420,
                            YearOfManufacture = 2016,
                        },
                    },
                    new FavoriteProduct
                    {
                        Product = new Product
                        {
                            Name = "Kira",
                            Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                            Price = 420,
                            PriceWithGeneralDiscount = 420,
                            YearOfManufacture = 2016,
                        },
                    },
                },
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var mockCountriesService = new Mock<ICountriesService>();
            var mockWishlistsService = new Mock<IWishlistService>();
            var mockOrdersService = new Mock<IOrdersService>();
            var mockCommentsService = new Mock<ICommentsService>();
            var mockCommentVotesService = new Mock<ICommentVotesService>();
            var mockNotificationsService = new Mock<INotificationsService>();

            var profileService = new ProfileService(
                new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                new EfDeletableEntityRepository<ApplicationRole>(dbContext),
                mockCountriesService.Object,
                mockWishlistsService.Object,
                mockOrdersService.Object,
                mockCommentsService.Object,
                mockCommentVotesService.Object,
                mockNotificationsService.Object,
                null);

            // Act
            var actualJson = await profileService.GetPersonalDataForUserJsonAsync(user.Id);

            // Assert
            Assert.NotNull(actualJson);
        }

        [Fact]
        public async Task GetUserByIdTheResultShouldBeUser()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var user = new ApplicationUser
            {
                FirstName = "FirstName",
                LastName = "LastName",
                UserName = "aa@example.com",
                Email = "aa@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            userManager
                .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser
                {
                    FirstName = "FirstName",
                    LastName = "LastName",
                    UserName = "aa@example.com",
                    Email = "aa@example.com",
                    Town = "Test",
                    PostalCode = "1000",
                    Address = "Test",
                });

            var profileService = new ProfileService(
                    new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    userManager.Object);

            // Act
            var resultUser = await userManager.Object.FindByIdAsync(user.Id);

            // Assert
            Assert.NotNull(resultUser);
            Assert.Equal(user.FirstName, resultUser.FirstName);
            Assert.Equal(user.LastName, resultUser.LastName);
            Assert.Equal(user.UserName, resultUser.UserName);
            Assert.Equal(user.Email, resultUser.Email);
            Assert.Equal(user.Town, resultUser.Town);
            Assert.Equal(user.PostalCode, resultUser.PostalCode);
            Assert.Equal(user.Address, resultUser.Address);
        }

        [Fact]
        public async Task GetDetailsForUserEditTheResultShouldBeInputModel()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var user = new ApplicationUser
            {
                FirstName = "FirstName",
                LastName = "LastName",
                UserName = "aa@example.com",
                Email = "aa@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var mockCountriesService = new Mock<ICountriesService>();

            var profileService = new ProfileService(
                    new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                    null,
                    mockCountriesService.Object,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);

            // Act
            var resultUser = await profileService.GetDetailsForUserEditAsync(user.Id);

            // Assert
            Assert.NotNull(resultUser);
            Assert.Equal(user.FirstName, resultUser.FirstName);
            Assert.Equal(user.LastName, resultUser.LastName);
            Assert.Equal(user.UserName, resultUser.UserName);
            Assert.Equal(user.Email, resultUser.Email);
            Assert.Equal(user.Town, resultUser.Town);
            Assert.Equal(user.PostalCode, resultUser.PostalCode);
            Assert.Equal(user.Address, resultUser.Address);
        }

        [Fact]
        public async Task EditUserDetailsTheResultShouldBeTrue()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var user = new ApplicationUser
            {
                FirstName = "FirstName",
                LastName = "LastName",
                UserName = "aa@example.com",
                Email = "aa@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var inputModel = new UserEditInputModel
            {
                FirstName = "Toshko",
                LastName = "Galabov",
                UserName = "tg@example.com",
                Email = "tg@example.com",
                Town = "Sofia",
                PostalCode = "1000",
                Address = "ul. Kap. P. Voivoda",
            };

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            userManager
                .Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            var profileService = new ProfileService(
                    new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    userManager.Object);

            // Act
            var result = await profileService.EditUserDetailsAsync(user, inputModel);
            var resultUser = await dbContext.Users.SingleOrDefaultAsync(u => u.Id == user.Id);

            // Assert
            Assert.True(result);
            Assert.Equal(user.FirstName, resultUser.FirstName);
            Assert.Equal(user.LastName, resultUser.LastName);
            Assert.Equal(user.UserName, resultUser.UserName);
            Assert.Equal(user.Email, resultUser.Email);
            Assert.Equal(user.Town, resultUser.Town);
            Assert.Equal(user.PostalCode, resultUser.PostalCode);
            Assert.Equal(user.Address, resultUser.Address);
        }

        // TODO: GetAllUsersExceptAdminsAsync
    }
}
