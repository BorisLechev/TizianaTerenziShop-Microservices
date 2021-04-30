namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Data.Comments;
    using TizianaTerenzi.Services.Data.Countries;
    using TizianaTerenzi.Services.Data.Orders;
    using TizianaTerenzi.Services.Data.PersonalData;
    using TizianaTerenzi.Services.Data.Votes;
    using TizianaTerenzi.Services.Data.Wishlist;
    using Xunit;

    public class PersonalDataServiceTests
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
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var mockCountriesService = new Mock<ICountriesService>();
            var mockWishlistsService = new Mock<IWishlistService>();
            var mockOrdersService = new Mock<IOrdersService>();
            var mockCommentsService = new Mock<ICommentsService>();
            var mockCommentVotesService = new Mock<ICommentVotesService>();

            var personalDataService = new PersonalDataService(new EfDeletableEntityRepository<ApplicationUser>(dbContext), mockCountriesService.Object, mockWishlistsService.Object, mockOrdersService.Object, mockCommentsService.Object, mockCommentVotesService.Object, null);
            var commentsService = new CommentsService(new EfDeletableEntityRepository<Comment>(dbContext));
            var wishlistService = new WishlistService(new EfDeletableEntityRepository<FavoriteProduct>(dbContext));

            Assert.Equal(1, await dbContext.Users.CountAsync());
            Assert.Equal(2, await dbContext.Comments.CountAsync());
            Assert.Equal(2, await dbContext.FavoriteProducts.CountAsync());

            var userResult = await personalDataService.DeleteUserAsync(user.Id);
            Assert.True(userResult);
            Assert.Equal(0, await dbContext.Users.CountAsync());

            var commentsResult = await commentsService.DeleteRangeByUserIdAsync(user.Id);
            Assert.True(commentsResult);
            Assert.Equal(0, await dbContext.Comments.CountAsync());

            var wishlistResult = await wishlistService.DeleteAllProductsInTheWishlistAsync(user.Id);
            Assert.True(wishlistResult);
            Assert.Equal(0, await dbContext.FavoriteProducts.CountAsync());
        }

        [Fact]
        public async Task DeleteUserWithNonExistentUserReturnsFalse()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var userId = Guid.NewGuid().ToString();

            var personalDataService = new PersonalDataService(new EfDeletableEntityRepository<ApplicationUser>(dbContext), null, null, null, null, null, null);

            // Act
            var result = await personalDataService.DeleteUserAsync(userId);

            // Assert
            Assert.False(result);
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

            var personalDataService = new PersonalDataService(new EfDeletableEntityRepository<ApplicationUser>(dbContext), mockCountriesService.Object, mockWishlistsService.Object, mockOrdersService.Object, mockCommentsService.Object, mockCommentVotesService.Object, null);

            // Act
            var result = await personalDataService.DeleteUserAsync(null);

            // Assert
            Assert.False(result);
        }

        //[Fact]
        //public async Task GetPersonalDataForUserJsonWithCorrectUserIdWorksCorrectly()
        //{
        //    const string expectedJson =
        //       "{\"FirstName\": \"FirstName\",\"LastName\": \"LastName\",\"Email\": \"aa@example.com\"," +
        //       "\"Comments\": [{\"Content\": \"first comment\"}],\"FavoriteProduct\": " +
        //       "[{\"ProductName\": \"Kira\"}]";

        //    var dateTime = new DateTime(2015, 1, 1, 10, 25, 30);

        //    var user = new ApplicationUser
        //    {
        //        FirstName = "FirstName",
        //        LastName = "LastName",
        //        UserName = "aa@example.com",
        //        Email = "aa@example.com",
        //        Town = "Test",
        //        PostalCode = "1000",
        //        Address = "Test",
        //        Comments = new List<Comment>
        //        {
        //            new Comment
        //            {
        //                Product = new Product
        //                {
        //                    Name = "Telea",
        //                    Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
        //                    Price = 320,
        //                    PriceWithDiscount = 320,
        //                    YearOfManufacture = 2015,
        //                },
        //                Content = "first comment",
        //            },
        //            new Comment
        //            {
        //                Product = new Product
        //                {
        //                    Name = "Telea",
        //                    Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
        //                    Price = 320,
        //                    PriceWithDiscount = 320,
        //                    YearOfManufacture = 2015,
        //                },
        //                Content = "first comment",
        //            },
        //        },
        //        FavoriteProducts = new List<FavoriteProduct>
        //        {
        //            new FavoriteProduct
        //            {
        //                Product = new Product
        //                {
        //                    Name = "Kira",
        //                    Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
        //                    Price = 420,
        //                    PriceWithDiscount = 420,
        //                    YearOfManufacture = 2016,
        //                },
        //            },
        //            new FavoriteProduct
        //            {
        //                Product = new Product
        //                {
        //                    Name = "Kira",
        //                    Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
        //                    Price = 420,
        //                    PriceWithDiscount = 420,
        //                    YearOfManufacture = 2016,
        //                },
        //            },
        //        },
        //    };

        //    // Arrange
        //    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString());
        //    var dbContext = new ApplicationDbContext(optionsBuilder.Options);
        //    var service = new PersonalDataService(new EfDeletableEntityRepository<ApplicationUser>(dbContext), null, null, null, null, null, null);

        //    await dbContext.Users.AddAsync(user);
        //    await dbContext.SaveChangesAsync();

        //    var mockCountriesService = new Mock<ICountriesService>();
        //    var mockWishlistsService = new Mock<IWishlistService>();
        //    var mockOrdersService = new Mock<IOrdersService>();
        //    var mockCommentsService = new Mock<ICommentsService>();
        //    var mockCommentVotesService = new Mock<ICommentVotesService>();

        //    var personalDataService = new PersonalDataService(new EfDeletableEntityRepository<ApplicationUser>(dbContext), mockCountriesService.Object, mockWishlistsService.Object, mockOrdersService.Object, mockCommentsService.Object, mockCommentVotesService.Object, null);

        //    // Act
        //    var actualJson = await personalDataService.GetPersonalDataForUserJsonAsync(user.Id);

        //    // Assert
        //    var expectedResult = JToken.Parse(expectedJson);
        //    var actualResult = JToken.Parse(actualJson);

        //    var equal = JToken.DeepEquals(expectedResult, actualResult);

        //    Assert.True(equal);
        //}
    }
}
