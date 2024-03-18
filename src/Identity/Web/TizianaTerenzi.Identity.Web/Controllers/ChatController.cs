namespace TizianaTerenzi.Identity.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common.Web.Controllers;
    using TizianaTerenzi.Identity.Data.Models;
    using TizianaTerenzi.Identity.Services.Data.Chat;
    using TizianaTerenzi.Identity.Web.Models.Chat;

    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ChatController : ApiController
    {
        private readonly IChatService chatsService;

        private readonly IEmojisService emojisService;

        private readonly UserManager<ApplicationUser> userManager;

        public ChatController(
            IChatService chatsService,
            IEmojisService emojisService,
            UserManager<ApplicationUser> userManager)
        {
            this.chatsService = chatsService;
            this.emojisService = emojisService;
            this.userManager = userManager;
        }

        public async Task<ActionResult<ChatViewModel>> Index(string username, string groupId)
        {
            var sender = await this.userManager.GetUserAsync(this.User);
            var receiver = await this.userManager.FindByNameAsync(username);

            var allMessages = await this.chatsService.GetAllMessagesByGroupIdAsync(groupId);
            var allEmojis = await this.emojisService.GetAllEmojisAsync();

            var viewModel = new ChatViewModel
            {
                SenderId = sender.Id,
                SenderUsername = sender.UserName,
                ReceiverId = receiver.Id,
                ReceiverUsername = receiver.UserName,
                ChatMessages = allMessages,
                GroupId = groupId,
                Emojis = allEmojis,
            };

            return this.Ok(viewModel);
        }
    }
}
