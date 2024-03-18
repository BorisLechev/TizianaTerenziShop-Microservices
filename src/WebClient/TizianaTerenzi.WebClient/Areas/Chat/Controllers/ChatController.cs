namespace TizianaTerenzi.WebClient.Areas.Chat.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.WebClient.Controllers;
    using TizianaTerenzi.WebClient.Services.Identity;

    [Authorize]
    [Area("Chat")]
    public class ChatController : BaseController
    {
        private readonly IIdentityService identityService;

        public ChatController(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        public async Task<IActionResult> Index(string username, string groupId)
        {
            var allMessages = await this.identityService.GetAllMessagesByGroupIdAsync(username, groupId);

            return this.View(allMessages);
        }
    }
}
