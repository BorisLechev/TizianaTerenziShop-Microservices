namespace TizianaTerenzi.Notifications.Web.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Notifications.Services.Data.CartNotifications;

    //[Authorize]
    public class NumberOfProductsInTheUsersCartHub : Hub
    {
        private readonly ICartNotificationsService cartNotificationsService;

        public NumberOfProductsInTheUsersCartHub(ICartNotificationsService cartNotificationsService)
        {
            this.cartNotificationsService = cartNotificationsService;
        }

        public async Task GetNumberOfProductsInTheUsersCart()
        {
            var isAuthenticated = this.Context.User.Identity.IsAuthenticated;
            int numberOfProductsInTheUsersCart = 0;

            if (isAuthenticated)
            {
                numberOfProductsInTheUsersCart = await this.cartNotificationsService.GetNumberOfProductsInTheUsersCartAsync(this.Context.User.GetUserId());
            }

            await this.Clients.Caller.SendAsync("NumberOfProductsInTheUsersCart", numberOfProductsInTheUsersCart);
        }

        public async Task AddProductInTheUsersCart(int numberOfProductsInTheUsersCart, string userId)
        {
            await this.Clients.User(userId).SendAsync("NumberOfProductsInTheUsersCart", numberOfProductsInTheUsersCart);

            var result = await this.cartNotificationsService.AddCartNotificationAsync(userId, numberOfProductsInTheUsersCart);
        }
    }
}
