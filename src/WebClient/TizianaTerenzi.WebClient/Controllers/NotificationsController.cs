namespace TizianaTerenzi.WebClient.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.WebClient.Services.Notifications;

    [Authorize]
    public class NotificationsController : BaseController
    {
        private readonly INotificationsService notificationsService;

        public NotificationsController(INotificationsService notificationsService)
        {
            this.notificationsService = notificationsService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await this.notificationsService.GetUserNotificationsAsync();

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<bool> Delete(string id)
        {
            var isDeleted = await this.notificationsService.DeleteNotificationAsync(id);

            return isDeleted;
        }
    }
}
