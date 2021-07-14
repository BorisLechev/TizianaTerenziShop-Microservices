namespace TizianaTerenzi.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Chat;
    using TizianaTerenzi.Web.ViewModels.Chat;

    public class ChatController : BaseController
    {
        private readonly IChatService chatService;

        private readonly UserManager<ApplicationUser> userManager;

        public ChatController(
            IChatService chatService,
            UserManager<ApplicationUser> userManager)
        {
            this.chatService = chatService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(string username, string groupId)
        {
            var sender = await this.userManager.GetUserAsync(this.User);
            var receiver = await this.userManager.FindByNameAsync(username);

            var allMessages = await this.chatService.GetAllMessagesByGroupIdAsync(groupId);

            var viewModel = new ChatViewModel
            {
                SenderId = sender.Id,
                SenderUsername = sender.UserName,
                ReceiverId = receiver.Id,
                ReceiverUsername = receiver.UserName,
                ChatMessages = allMessages,
                GroupId = groupId,
            };

            return this.View(viewModel);
        }
    }
}
