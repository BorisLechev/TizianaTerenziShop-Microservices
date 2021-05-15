namespace TizianaTerenzi.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
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

        [Route("chat/with/{username?}/group/{group?}")]
        public async Task<IActionResult> Index(string username, string group)
        {
            var sender = await this.userManager.GetUserAsync(this.User);
            var receiver = await this.userManager.FindByNameAsync(username);

            var isUserAbleToChat = await this.chatService.IsUserAbleToChatAsync(sender.UserName, group);

            if (isUserAbleToChat == false)
            {
                this.Error(NotificationMessages.NotAbleToChat);

                return this.LocalRedirect($"/profile/{sender.Id}");
            }

            var allMessages = await this.chatService.GetAllMessagesByGroupNameAsync(group);

            // TODO: IsUserAbleToChat
            var viewModel = new ChatViewModel
            {
                SenderId = sender.Id,
                SenderUsername = sender.UserName,
                ReceiverId = receiver.Id,
                ReceiverUsername = receiver.UserName,
                GroupName = group,
                ChatMessages = allMessages,
            };

            return this.View(viewModel);
        }
    }
}
