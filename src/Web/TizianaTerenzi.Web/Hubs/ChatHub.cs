namespace TizianaTerenzi.Web.Hubs
{
    using System.Threading.Tasks;

    using Ganss.XSS;
    using Microsoft.AspNetCore.SignalR;
    using TizianaTerenzi.Services.Data.Chat;

    public class ChatHub : Hub
    {
        private readonly IChatService chatService;

        public ChatHub(
            IChatService chatService)
        {
            this.chatService = chatService;
        }

        public async Task AddToGroup(string groupName, string receiverUsername, string senderUsername)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
            await this.chatService.AddUserToGroupAsync(groupName, receiverUsername, senderUsername);
        }

        public async Task SendMessage(string senderUsername, string receiverUsername, string message, string groupName)
        {
            string receiverId = await this.chatService.SendMessageToUserAsync(senderUsername, receiverUsername, message, groupName);

            await this.Clients.User(receiverId).SendAsync("ReceiveMessage", senderUsername, new HtmlSanitizer().Sanitize(message.Trim()));
        }

        public async Task ReceiveMessage(string senderUsername, string message, string groupName)
        {
            var senderId = await this.chatService.ReceiveNewMessageAsync(senderUsername, message, groupName);

            await this.Clients.User(senderId).SendAsync("SendMessage", senderUsername, message.Trim());
        }

        public async Task UserType(string senderUsername, string receiverUsername)
        {
            var receiverId = await this.chatService.UserTypeAsync(senderUsername, receiverUsername);

            await this.Clients.User(receiverId).SendAsync("VisualizeUserType", senderUsername, receiverUsername);
        }

        public async Task UserStopType(string receiverUsername)
        {
            var receiverId = await this.chatService.UserStopTypeAsync(receiverUsername);

            await this.Clients.User(receiverId).SendAsync("VisualizeUserStopType");
        }
    }
}
