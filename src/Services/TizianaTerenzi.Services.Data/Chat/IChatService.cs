namespace TizianaTerenzi.Services.Data.Chat
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.WebClient.ViewModels.Chat;

    public interface IChatService
    {
        Task<string> GetChatGroupByUserIdsAsync(string userId, string currentUserId);

        Task<string> AddUserToGroupAsync(string groupId, string receiversUsername, string sendersUsername);

        Task<string> SendMessageToUserAsync(string sendersUsername, string receiversUsername, string sanitizedMessage, string groupId);

        Task<ICollection<ChatMessageViewModel>> GetAllMessagesByGroupIdAsync(string groupId);
    }
}
