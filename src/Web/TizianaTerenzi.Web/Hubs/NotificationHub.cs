namespace TizianaTerenzi.Web.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Notifications;

    public class NotificationHub : Hub
    {
        private readonly INotificationsService notificationsService;

        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public NotificationHub(
            INotificationsService notificationsService,
            IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.notificationsService = notificationsService;
            this.usersRepository = usersRepository;
        }

        public async Task GetUserNotificationsCount(bool isFirstNotificationSound)
        {
            var username = this.Context.User.Identity.Name;

            if (username != null)
            {
                var targetUser = await this.usersRepository
                    .AllAsNoTracking()
                    .SingleOrDefaultAsync(u => u.UserName == username);

                var count = await this.notificationsService.GetUserNotificationsCountAsync(targetUser.UserName);

                await this.Clients.User(targetUser.Id).SendAsync("ReceiveNotification", count, isFirstNotificationSound);
            }
        }
    }
}
