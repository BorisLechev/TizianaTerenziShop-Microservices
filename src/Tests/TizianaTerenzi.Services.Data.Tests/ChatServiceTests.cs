namespace TizianaTerenzi.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Data.Repositories;
    using TizianaTerenzi.Services.Data.Chat;
    using Xunit;

    public class ChatServiceTests
    {
        [Fact]
        public async Task AddUserToChatGroupSuccessfully()
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

            var secondSenderUser = new ApplicationUser
            {
                FirstName = "FirstName4",
                LastName = "LastName4",
                UserName = "mail4@example.com",
                Email = "mail4@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
            };

            await dbContext.Users.AddRangeAsync(senderUser, receiverUser, secondSenderUser);
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

            var service = new ChatService(
                new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                new EfDeletableEntityRepository<ChatGroup>(dbContext),
                new EfDeletableEntityRepository<ChatMessage>(dbContext));

            // Act
            var returnChatGroupId = await service.AddUserToGroupAsync(chatGroup.Id, receiverUser.UserName, senderUser.UserName);
            var return2ChatGroupId = await service.AddUserToGroupAsync("123za", receiverUser.UserName, secondSenderUser.UserName);

            // Assert
            Assert.Equal(chatGroup.Id, returnChatGroupId);
            Assert.NotEqual(returnChatGroupId, return2ChatGroupId);
            Assert.Equal(chatGroup.Id, returnChatGroupId);
        }

        [Fact]
        public async Task GetChatGroupIdByUserIdsSuccessfully()
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

            var secondSenderUser = new ApplicationUser
            {
                FirstName = "FirstName4",
                LastName = "LastName4",
                UserName = "mail4@example.com",
                Email = "mail4@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
            };

            await dbContext.Users.AddRangeAsync(senderUser, receiverUser, secondSenderUser);
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

            var service = new ChatService(
                new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                new EfDeletableEntityRepository<ChatGroup>(dbContext),
                new EfDeletableEntityRepository<ChatMessage>(dbContext));

            // Act
            var returnChatGroupId = await service.AddUserToGroupAsync(chatGroup.Id, receiverUser.UserName, senderUser.UserName);
            var getChatGroupId = await service.GetChatGroupByUserIdsAsync(receiverUser.Id, senderUser.Id);

            // Assert
            Assert.Equal(chatGroup.Id, returnChatGroupId);
            Assert.Equal(returnChatGroupId, getChatGroupId);
            Assert.Equal(chatGroup.Id, returnChatGroupId);
        }

        [Fact]
        public async Task SendMessageInChatGroupSuccessfully()
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

            var secondSenderUser = new ApplicationUser
            {
                FirstName = "FirstName4",
                LastName = "LastName4",
                UserName = "mail4@example.com",
                Email = "mail4@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
            };

            await dbContext.Users.AddRangeAsync(senderUser, receiverUser, secondSenderUser);
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

            var service = new ChatService(
                new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                new EfDeletableEntityRepository<ChatGroup>(dbContext),
                new EfDeletableEntityRepository<ChatMessage>(dbContext));

            // Act
            var returnChatGroupId = await service.AddUserToGroupAsync(chatGroup.Id, receiverUser.UserName, senderUser.UserName);
            var receiversId = await service.SendMessageToUserAsync(
                senderUser.UserName,
                receiverUser.UserName,
                "tuk-tam",
                chatGroup.Id);

            // Assert
            Assert.Equal(chatGroup.Id, returnChatGroupId);
            Assert.Equal(chatGroup.Id, returnChatGroupId);
            Assert.Equal(receiverUser.Id, receiversId);
        }

        [Fact]
        public async Task GetAllChatMessagesFromChatGroupSuccessfully()
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

            var secondSenderUser = new ApplicationUser
            {
                FirstName = "FirstName4",
                LastName = "LastName4",
                UserName = "mail4@example.com",
                Email = "mail4@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
            };

            await dbContext.Users.AddRangeAsync(senderUser, receiverUser, secondSenderUser);
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

            var service = new ChatService(
                new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                new EfDeletableEntityRepository<ChatGroup>(dbContext),
                new EfDeletableEntityRepository<ChatMessage>(dbContext));

            // Act
            var returnChatGroupId = await service.AddUserToGroupAsync(chatGroup.Id, receiverUser.UserName, senderUser.UserName);
            var return2ChatGroupId = await service.AddUserToGroupAsync("123za", receiverUser.UserName, secondSenderUser.UserName);
            var getChatGroupId = await service.GetChatGroupByUserIdsAsync(receiverUser.Id, senderUser.Id);
            var receiversId = await service.SendMessageToUserAsync(
                senderUser.UserName,
                receiverUser.UserName,
                "tuk-tam",
                chatGroup.Id);
            var allMessages = await service.GetAllMessagesByGroupIdAsync(chatGroup.Id);
            var allMessagesFromSecondGroup = await service.GetAllMessagesByGroupIdAsync(return2ChatGroupId);

            // Assert
            Assert.Equal(chatGroup.Id, returnChatGroupId);
            Assert.Equal(returnChatGroupId, getChatGroupId);
            Assert.NotEqual(returnChatGroupId, return2ChatGroupId);
            Assert.Equal(chatGroup.Id, returnChatGroupId);
            Assert.Equal(receiverUser.Id, receiversId);
            Assert.Single(allMessages);
            Assert.Equal("tuk-tam", allMessages.First().Content);
            Assert.Equal(senderUser.UserName, allMessages.First().AuthorUserName);
            Assert.Empty(allMessagesFromSecondGroup);
        }

        [Fact]
        public async Task GetAllMessagesFromNotexistingChatGroupShouldReturnNullCollection()
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

            var secondSenderUser = new ApplicationUser
            {
                FirstName = "FirstName4",
                LastName = "LastName4",
                UserName = "mail4@example.com",
                Email = "mail4@example.com",
                Town = "Test",
                PostalCode = "1000",
                Address = "Test",
            };

            await dbContext.Users.AddRangeAsync(senderUser, receiverUser, secondSenderUser);
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

            var service = new ChatService(
                new EfDeletableEntityRepository<ApplicationUser>(dbContext),
                new EfDeletableEntityRepository<ChatGroup>(dbContext),
                new EfDeletableEntityRepository<ChatMessage>(dbContext));

            // Act
            var returnChatGroupId = await service.AddUserToGroupAsync(chatGroup.Id, receiverUser.UserName, senderUser.UserName);
            var return2ChatGroupId = await service.AddUserToGroupAsync("123za", receiverUser.UserName, secondSenderUser.UserName);
            var getChatGroupId = await service.GetChatGroupByUserIdsAsync(receiverUser.Id, senderUser.Id);
            var receiversId = await service.SendMessageToUserAsync(
                senderUser.UserName,
                receiverUser.UserName,
                "tuk-tam",
                chatGroup.Id);

            // Assert
            Assert.Null(await service.GetAllMessagesByGroupIdAsync("not-existing-id"));
        }
    }
}
