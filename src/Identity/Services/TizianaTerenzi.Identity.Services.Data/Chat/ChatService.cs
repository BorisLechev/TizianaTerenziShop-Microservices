namespace TizianaTerenzi.Identity.Services.Data.Chat
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Notifications;
    using TizianaTerenzi.Common.Services.Mapping;
    using TizianaTerenzi.Identity.Data.Models;
    using TizianaTerenzi.Identity.Web.Models.Chat;

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

        public async Task<string> GetChatGroupByUserIdsAsync(string userId, string currentUserId)
        {
            var chatGroup = await this.chatGroupsRepository
                                .All()
                                .Select(x => new
                                {
                                    ChatGroupId = x.Id,
                                    ChatUserGroups = x.ChatUserGroups,
                                })
                                .SingleOrDefaultAsync(x =>
                                    x.ChatUserGroups.Select(x => x.UserId).Contains(userId) &&
                                    x.ChatUserGroups.Select(x => x.UserId).Contains(currentUserId));

            return chatGroup?.ChatGroupId;
        }

        public async Task<string> AddUserToGroupAsync(string groupId, string receiversUsername, string sendersUsername)
        {
            var chatGroup = await this.chatGroupsRepository
                                .All()
                                .SingleOrDefaultAsync(cg => cg.Id == groupId);

            if (chatGroup == null)
            {
                var receiver = await this.usersRepository
                                    .All()
                                    .SingleOrDefaultAsync(u => u.UserName == receiversUsername);

                var sender = await this.usersRepository
                                    .All()
                                    .SingleOrDefaultAsync(u => u.UserName == sendersUsername);

                chatGroup = new ChatGroup();

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

            return chatGroup.Id;
        }

        public async Task<ICollection<ChatMessageViewModel>> GetAllMessagesByGroupIdAsync(string groupId)
        {
            var isExisting = await this.chatGroupsRepository
                                    .All()
                                    .AnyAsync(g => g.Id == groupId);

            if (isExisting)
            {
                var messages = await this.chatMessagesRepository
                                    .All()
                                    .Where(cm => cm.ChatGroupId == groupId)
                                    .To<ChatMessageViewModel>()
                                    .OrderBy(m => m.CreatedOn)
                                    .ToListAsync();

                return messages;
            }

            return null;
        }

        public async Task<string> SendMessageToUserAsync(ChatMessageToUserSentMessage message)
        {
            var receiversId = await this.usersRepository
                                    .All()
                                    .Where(u => u.UserName == message.ReceiversUsername)
                                    .Select(u => u.Id)
                                    .SingleOrDefaultAsync();

            var sender = await this.usersRepository
                                .All()
                                .SingleOrDefaultAsync(u => u.UserName == message.SendersUsername);

            var group = await this.chatGroupsRepository
                            .All()
                            .SingleOrDefaultAsync(g => g.Id == message.GroupId);

            var newMessage = new ChatMessage
            {
                Author = sender,
                ChatGroup = group,
                Content = message.SanitizedMessage,
            };

            await this.chatMessagesRepository.AddAsync(newMessage);
            await this.chatMessagesRepository.SaveChangesAsync();

            return receiversId;
        }

        // TODO: DeleteChatGroupWithMessagesAsync
    }
}
