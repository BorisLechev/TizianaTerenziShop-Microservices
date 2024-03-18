namespace TizianaTerenzi.Notifications.Web.Hubs
{
    using Microsoft.AspNetCore.SignalR;

    public class UserStatusHub : Hub
    {
        private static readonly ICollection<string> OnlineUsers = new HashSet<string>();

        public override async Task OnConnectedAsync()
        {
            var username = this.Context.User.Identity.Name;

            if (username != null)
            {
                OnlineUsers.Add(username);

                await base.OnConnectedAsync();
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var username = this.Context.User.Identity.Name;

            if (username != null)
            {
                OnlineUsers.Remove(username);

                await base.OnDisconnectedAsync(exception);
            }
        }

        public async Task IsUserOnline(string username)
        {
            if (OnlineUsers.Contains(username))
            {
                await this.Clients.All.SendAsync("UserIsOnlineStatusDot", username);
            }
            else
            {
                await this.Clients.All.SendAsync("UserIsOfflineStatusDot", username);
            }
        }
    }
}
