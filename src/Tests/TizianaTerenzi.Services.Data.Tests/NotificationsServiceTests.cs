namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Data.Notifications;
    using Xunit;

    public class NotificationsServiceTests
    {
        [Fact]
        public async Task AddMessageNotificationsShouldWorkCorrectly()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var senderUser = new ApplicationUser
            {
                FirstName = "FirstName2",
                LastName = "LastName2",
                UserName = "mail2@example.com",
                Email = "mail1@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
            };

            var receiverUser = new ApplicationUser
            {
                FirstName = "FirstName3",
                LastName = "LastName3",
                UserName = "mail3@example.com",
                Email = "mail3@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
            };

            await dbContext.Users.AddRangeAsync(senderUser, receiverUser);
            await dbContext.SaveChangesAsync();

            var chatGroup = new ChatGroup();

            var groupReceiver = new ChatUserGroup
            {
                ChatGroup = chatGroup,
                User = receiverUser,
            };

            var groupSender = new ChatUserGroup
            {
                ChatGroup = chatGroup,
                User = senderUser,
            };

            chatGroup.ChatUserGroups.Add(groupReceiver);
            chatGroup.ChatUserGroups.Add(groupSender);

            await dbContext.ChatGroups.AddAsync(chatGroup);
            await dbContext.SaveChangesAsync();

            var list = new List<ApplicationUserNotification>();

            for (int i = 0; i < GlobalConstants.MaxChatNotificationsPerUser; i++)
            {
                var notification = new ApplicationUserNotification
                {
                    ReceiverUsername = receiverUser.UserName,
                    SenderId = senderUser.Id,
                    Text = $"test{i}",
                    Link = $"/chat/with/{senderUser.UserName}/group/{chatGroup.Id}",
                };

                list.Add(notification);
            }

            await dbContext.UserNotifications.AddRangeAsync(list);
            await dbContext.SaveChangesAsync();

            var service = new NotificationsService(
                new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                new EfDeletableEntityRepository<ApplicationUserNotification>(dbContext));

            // Act
            var receiverNotificationsCount = await service.GetUserNotificationsCountAsync(receiverUser.UserName);
            var notificationId = await service.AddMessageNotificationAsync(senderUser.UserName, receiverUser.UserName, "bla", chatGroup.Id);
            var receiverNotificationsCountAfterDeletion = await service.GetUserNotificationsCountAsync(receiverUser.UserName);

            // Assert
            Assert.Equal(GlobalConstants.MaxChatNotificationsPerUser, receiverNotificationsCount);
            Assert.NotNull(notificationId);
            Assert.Equal(2, receiverNotificationsCountAfterDeletion);
        }

        [Fact]
        public async Task GetMessageNotificationByIdShouldWorkCorrectly()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var senderUser = new ApplicationUser
            {
                FirstName = "FirstName2",
                LastName = "LastName2",
                UserName = "mail2@example.com",
                Email = "mail1@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
            };

            var receiverUser = new ApplicationUser
            {
                FirstName = "FirstName3",
                LastName = "LastName3",
                UserName = "mail3@example.com",
                Email = "mail3@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
            };

            await dbContext.Users.AddRangeAsync(senderUser, receiverUser);
            await dbContext.SaveChangesAsync();

            var chatGroup = new ChatGroup();

            var groupReceiver = new ChatUserGroup
            {
                ChatGroup = chatGroup,
                User = receiverUser,
            };

            var groupSender = new ChatUserGroup
            {
                ChatGroup = chatGroup,
                User = senderUser,
            };

            chatGroup.ChatUserGroups.Add(groupReceiver);
            chatGroup.ChatUserGroups.Add(groupSender);

            await dbContext.ChatGroups.AddAsync(chatGroup);
            await dbContext.SaveChangesAsync();

            var notification = new ApplicationUserNotification
            {
                ReceiverUsername = receiverUser.UserName,
                SenderId = senderUser.Id,
                Text = "test",
                Link = $"/chat/with/{senderUser.UserName}/group/{chatGroup.Id}",
            };

            await dbContext.UserNotifications.AddAsync(notification);
            await dbContext.SaveChangesAsync();

            var service = new NotificationsService(
                new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                new EfDeletableEntityRepository<ApplicationUserNotification>(dbContext));

            // Act
            var notificationId = await service.AddMessageNotificationAsync(senderUser.UserName, receiverUser.UserName, "bla", chatGroup.Id);
            var receiverNotificationsCount = await service.GetUserNotificationsCountAsync(receiverUser.UserName);
            var getNotificationById = await service.GetNotificationByIdAsync(notification.Id);

            // Assert
            Assert.Equal(2, receiverNotificationsCount);
            Assert.NotNull(notificationId);
            Assert.Equal(notification.Id, getNotificationById.Id);
            Assert.Equal(notification.SenderId, getNotificationById.SenderId);
            Assert.Equal(notification.Text, getNotificationById.Text);
            Assert.Equal(notification.Link, getNotificationById.Link);
            Assert.Null(await service.GetNotificationByIdAsync("null"));
        }

        [Fact]
        public async Task GetAllMessageNotificationsShouldWorkCorrectly()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var senderUser = new ApplicationUser
            {
                FirstName = "FirstName2",
                LastName = "LastName2",
                UserName = "mail2@example.com",
                Email = "mail1@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
            };

            var receiverUser = new ApplicationUser
            {
                FirstName = "FirstName3",
                LastName = "LastName3",
                UserName = "mail3@example.com",
                Email = "mail3@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
            };

            await dbContext.Users.AddRangeAsync(senderUser, receiverUser);
            await dbContext.SaveChangesAsync();

            var chatGroup = new ChatGroup();

            var groupReceiver = new ChatUserGroup
            {
                ChatGroup = chatGroup,
                User = receiverUser,
            };

            var groupSender = new ChatUserGroup
            {
                ChatGroup = chatGroup,
                User = senderUser,
            };

            chatGroup.ChatUserGroups.Add(groupReceiver);
            chatGroup.ChatUserGroups.Add(groupSender);

            await dbContext.ChatGroups.AddAsync(chatGroup);
            await dbContext.SaveChangesAsync();

            var list = new List<ApplicationUserNotification>();

            for (int i = 0; i < GlobalConstants.MaxChatNotificationsPerUser; i++)
            {
                var notification = new ApplicationUserNotification
                {
                    ReceiverUsername = receiverUser.UserName,
                    SenderId = senderUser.Id,
                    Text = $"test{i}",
                    Link = $"/chat/with/{senderUser.UserName}/group/{chatGroup.Id}",
                };

                list.Add(notification);
            }

            await dbContext.UserNotifications.AddRangeAsync(list);
            await dbContext.SaveChangesAsync();

            var service = new NotificationsService(
                new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                new EfDeletableEntityRepository<ApplicationUserNotification>(dbContext));

            // Act
            var allReceiversNotifications = await service.GetUserNotificationsAsync(receiverUser.UserName);
            var allSendersNotifications = await service.GetUserNotificationsAsync(senderUser.UserName);
            var nonExistingUserName = await service.GetUserNotificationsAsync("null");

            // Assert
            Assert.Equal(GlobalConstants.MaxChatNotificationsPerUser, allReceiversNotifications.Count());
            Assert.NotEqual(GlobalConstants.MaxChatNotificationsPerUser, allSendersNotifications.Count());
            Assert.Empty(nonExistingUserName);
        }

        [Fact]
        public async Task DeleteUserNotificationShouldWorkCorrectly()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var senderUser = new ApplicationUser
            {
                FirstName = "FirstName2",
                LastName = "LastName2",
                UserName = "mail2@example.com",
                Email = "mail1@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
            };

            var receiverUser = new ApplicationUser
            {
                FirstName = "FirstName3",
                LastName = "LastName3",
                UserName = "mail3@example.com",
                Email = "mail3@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
            };

            await dbContext.Users.AddRangeAsync(senderUser, receiverUser);
            await dbContext.SaveChangesAsync();

            var chatGroup = new ChatGroup();

            var groupReceiver = new ChatUserGroup
            {
                ChatGroup = chatGroup,
                User = receiverUser,
            };

            var groupSender = new ChatUserGroup
            {
                ChatGroup = chatGroup,
                User = senderUser,
            };

            chatGroup.ChatUserGroups.Add(groupReceiver);
            chatGroup.ChatUserGroups.Add(groupSender);

            await dbContext.ChatGroups.AddAsync(chatGroup);
            await dbContext.SaveChangesAsync();

            var notificationOne = new ApplicationUserNotification
            {
                ReceiverUsername = receiverUser.UserName,
                SenderId = senderUser.Id,
                Text = "test1",
                Link = $"/chat/with/{senderUser.UserName}/group/{chatGroup.Id}",
            };
            var notificationTwo = new ApplicationUserNotification
            {
                ReceiverUsername = receiverUser.UserName,
                SenderId = senderUser.Id,
                Text = "test2",
                Link = $"/chat/with/{senderUser.UserName}/group/{chatGroup.Id}",
            };

            await dbContext.UserNotifications.AddRangeAsync(notificationOne, notificationTwo);
            await dbContext.SaveChangesAsync();

            var service = new NotificationsService(
                new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                new EfDeletableEntityRepository<ApplicationUserNotification>(dbContext));

            // Act
            var allReceiversNotificationsCount = await service.GetUserNotificationsCountAsync(receiverUser.UserName);
            var deleteNotificationMustReturnTrue = await service.DeleteNotificationAsync(notificationOne.Id);
            var deleteNotificationWithNonExistingId = await service.DeleteNotificationAsync("null");
            var allReceiversNotificationsAfterDeletion = await service.GetUserNotificationsCountAsync(receiverUser.UserName);
            var allReceiversNotifications = await service.GetUserNotificationsAsync(receiverUser.UserName);

            // Assert
            Assert.Equal(2, allReceiversNotificationsCount);
            Assert.False(deleteNotificationWithNonExistingId);
            Assert.True(deleteNotificationMustReturnTrue);
            Assert.Equal(1, allReceiversNotificationsAfterDeletion);
            Assert.Equal(notificationTwo.SenderId, allReceiversNotifications.First().SenderId);
            Assert.Equal(notificationTwo.Text, allReceiversNotifications.First().Text);
            Assert.Equal(notificationTwo.Link, allReceiversNotifications.First().Link);
        }
    }
}
