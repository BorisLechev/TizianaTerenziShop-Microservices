namespace TizianaTerenzi.Notifications.Web.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using TizianaTerenzi.Notifications.Services.Carts;

    // [Authorize]
    public class NumberOfProductsInTheUsersCartHub : Hub
    {
        private readonly ICartsService cartsService;

        public NumberOfProductsInTheUsersCartHub(ICartsService cartsService)
        {
            this.cartsService = cartsService;
        }

        public async Task GetNumberOfProductsInTheUsersCart()
        {
            var isAuthenticated = this.Context.User.Identity.IsAuthenticated;
            int numberOfProductsInTheUsersCart = 0;

            if (isAuthenticated)
            {
                numberOfProductsInTheUsersCart = await this.cartsService.GetNumberOfProductsInTheUsersCart();
            }

            await this.Clients.Caller.SendAsync("NumberOfProductsInTheUsersCart", numberOfProductsInTheUsersCart);
        }
    }
}
