namespace TizianaTerenzi.Services.Data.Chat
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TizianaTerenzi.Web.ViewModels.Chat;

    public interface IChatService
    {
        Task AddUserToGroupAsync(string groupName, string receiversUsername, string sendersUsername);

        Task<string> SendMessageToUserAsync(string sendersUsername, string receiversUsername, string message, string groupName);

        Task<string> ReceiveNewMessageAsync(string sendersUsername, string message, string groupName);

        Task<ICollection<ChatMessageViewModel>> GetAllMessagesByGroupNameAsync(string groupName);

        Task<string> UserTypeAsync(string sendersUsername, string receiversUsername);

        Task<string> UserStopTypeAsync(string receiversUsername);

        Task<bool> IsUserAbleToChatAsync(string myUsername, string groupName);
    }
}
