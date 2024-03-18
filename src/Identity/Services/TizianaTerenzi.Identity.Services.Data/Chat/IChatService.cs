namespace TizianaTerenzi.Identity.Services.Data.Chat
{
    using TizianaTerenzi.Common.Messages.Notifications;
    using TizianaTerenzi.Identity.Web.Models.Chat;

    public interface IChatService
    {
        Task<string> GetChatGroupByUserIdsAsync(string userId, string currentUserId);

        Task<string> AddUserToGroupAsync(string groupId, string receiversUsername, string sendersUsername);

        Task<string> SendMessageToUserAsync(ChatMessageToUserSentMessage message);

        Task<ICollection<ChatMessageViewModel>> GetAllMessagesByGroupIdAsync(string groupId);
    }
}
