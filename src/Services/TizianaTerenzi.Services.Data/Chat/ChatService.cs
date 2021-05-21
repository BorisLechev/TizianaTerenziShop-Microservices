namespace TizianaTerenzi.Services.Data.Chat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Ganss.XSS;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Mapping;
    using TizianaTerenzi.Web.ViewModels.Chat;
    using Z.EntityFramework.Plus;

    public class ChatService : IChatService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        private readonly IDeletableEntityRepository<ChatGroup> chatGroupsRepository;

        private readonly IDeletableEntityRepository<ChatMessage> chatMessagesRepository;

        public ChatService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<ChatGroup> chatGroupsRepository,
            IDeletableEntityRepository<ChatMessage> chatMessagesRepository)
        {
            this.usersRepository = usersRepository;
            this.chatGroupsRepository = chatGroupsRepository;
            this.chatMessagesRepository = chatMessagesRepository;
        }

        public async Task AddUserToGroupAsync(string groupName, string receiversUsername, string sendersUsername)
        {
            var chatGroup = await this.chatGroupsRepository
                .All()
                .SingleOrDefaultAsync(cg => cg.Name == groupName);

            if (chatGroup == null)
            {
                var receiver = await this.usersRepository
                    .All()
                    .SingleOrDefaultAsync(u => u.UserName == receiversUsername);

                var sender = await this.usersRepository
                    .All()
                    .SingleOrDefaultAsync(u => u.UserName == sendersUsername);

                chatGroup = new ChatGroup
                {
                    Name = groupName,
                };

                var groupReceiver = new ChatUserGroup
                {
                    ChatGroup = chatGroup,
                    User = receiver,
                };

                var groupSender = new ChatUserGroup
                {
                    ChatGroup = chatGroup,
                    User = sender,
                };

                chatGroup.ChatUserGroups.Add(groupReceiver);
                chatGroup.ChatUserGroups.Add(groupSender);

                await this.chatGroupsRepository.AddAsync(chatGroup);
                await this.chatGroupsRepository.SaveChangesAsync();
            }
        }

        public async Task<ICollection<ChatMessageViewModel>> GetAllMessagesByGroupNameAsync(string groupName)
        {
            var groupId = await this.chatGroupsRepository
                .AllAsNoTracking()
                .Where(g => g.Name.ToLower() == groupName.ToLower())
                .Select(g => g.Id)
                .SingleOrDefaultAsync();

            if (groupId != null)
            {
                var messages = await this.chatMessagesRepository
                    .AllAsNoTracking()
                    .Where(cm => cm.ChatGroupId == groupId)
                    .To<ChatMessageViewModel>()
                    .OrderBy(m => m.CreatedOn)
                    .ToListAsync();

                return messages;
            }

            return null;
        }

        public async Task<string> ReceiveNewMessageAsync(string sendersUsername, string message, string groupName)
        {
            var sendersId = await this.usersRepository
                .AllAsNoTracking()
                .Where(u => u.UserName == sendersUsername)
                .Select(u => u.Id)
                .SingleOrDefaultAsync();

            return sendersId;
        }

        public async Task<string> SendMessageToUserAsync(string sendersUsername, string receiversUsername, string message, string groupName)
        {
            var receiversId = await this.usersRepository
                .All()
                .Where(u => u.UserName == receiversUsername)
                .Select(u => u.Id)
                .SingleOrDefaultAsync();

            var sender = await this.usersRepository
                .All()
                .SingleOrDefaultAsync(u => u.UserName == sendersUsername);

            var group = await this.chatGroupsRepository
                .All()
                .SingleOrDefaultAsync(g => g.Name.ToLower() == groupName.ToLower());

            var newMessage = new ChatMessage
            {
                Author = sender,
                ChatGroup = group,
                ReceiverUsername = receiversUsername,
                Content = new HtmlSanitizer().Sanitize(message.Trim()),
            };

            await this.chatMessagesRepository.AddAsync(newMessage);
            await this.chatMessagesRepository.SaveChangesAsync();

            return receiversId;
        }

        public async Task<string> UserTypeAsync(string sendersUsername, string receiversUsername)
        {
            var receiverId = await this.usersRepository
                .All()
                .Where(u => u.UserName.ToUpper() == receiversUsername.ToUpper())
                .Select(u => u.Id)
                .SingleOrDefaultAsync();

            return receiverId;
        }

        public async Task<string> UserStopTypeAsync(string receiversUsername)
        {
            var receiverId = await this.usersRepository
                .All()
                .Where(u => u.UserName.ToUpper() == receiversUsername.ToUpper())
                .Select(u => u.Id)
                .SingleOrDefaultAsync();

            return receiverId;
        }

        public async Task<bool> IsUserAbleToChatAsync(string myUsername, string groupName)
        {
            var groupMembers = groupName
                .Split(GlobalConstants.ChatGroupNameSeparator, StringSplitOptions.RemoveEmptyEntries);

            if (groupMembers.Contains(myUsername) == false)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteChatGroupWithMessagesAsync(string currentUserId, string currentUsername)
        {
            var chatGroups = await this.chatGroupsRepository
                .All()
                .Where(g => g.Name.Contains(currentUsername))
                .UpdateAsync(g => new ChatGroup
                {
                    IsDeleted = true,
                    DeletedOn = DateTime.UtcNow,
                });

            var chatMessages = await this.chatMessagesRepository
                .All()
                .Where(m => m.ReceiverUsername == currentUsername || m.AuthorId == currentUserId)
                .UpdateAsync(m => new ChatMessage
                {
                    IsDeleted = true,
                    DeletedOn = DateTime.UtcNow,
                });

            return chatGroups >= 0 && chatMessages >= 0;
        }
    }
}
