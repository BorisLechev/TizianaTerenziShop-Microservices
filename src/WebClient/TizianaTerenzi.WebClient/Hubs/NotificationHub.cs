namespace TizianaTerenzi.WebClient.Hubs
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data.Notifications;

    [Authorize]
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

        public async Task GetUserNotificationsCount(bool playNotificationSound)
        {
            var username = this.Context.User.Identity.Name;

            var receiver = await this.usersRepository
                .AllAsNoTracking()
                .Where(u => u.UserName == username)
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                })
                .SingleOrDefaultAsync();

            var count = await this.notificationsService.GetUserNotificationsCountAsync(receiver.UserName);

            await this.Clients
                .User(receiver.Id)
                .SendAsync("ReceiveNotification", count, playNotificationSound);
        }
    }
}
