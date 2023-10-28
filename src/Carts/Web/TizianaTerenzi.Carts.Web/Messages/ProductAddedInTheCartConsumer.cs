namespace TizianaTerenzi.Carts.Web.Messages
{
    using MassTransit;
    using Microsoft.AspNetCore.SignalR;
    using TizianaTerenzi.Carts.Services.Data.Carts;
    using TizianaTerenzi.Common.Messages.Products;
    using TizianaTerenzi.Notifications.Web.Hubs;

    public class ProductAddedInTheCartConsumer : IConsumer<ProductAddedInTheCartMessage>
    {
        private readonly ICartsService cartsService;
        private readonly IHubContext<NumberOfProductsInTheUsersCartHub> hubContext;

        public ProductAddedInTheCartConsumer(
            ICartsService cartsService,
            IHubContext<NumberOfProductsInTheUsersCartHub> hubContext)
        {
            this.cartsService = cartsService;
            this.hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<ProductAddedInTheCartMessage> context)
        {
            var message = context.Message;

            var ifProductInTheCartExists = await this.cartsService.CheckIfProductExistsInTheUsersCartAsync(message.UserId, message.ProductId);

            if (ifProductInTheCartExists)
            {
                var productInTheCartId = await this.cartsService.GetProductInTheCartIdByProductIdAsync(message.ProductId, message.UserId);

                await this.cartsService.IncreaseQuantityAsync(productInTheCartId);
            }
            else
            {
                await this.cartsService.AddProductInTheCartAsync(message);
            }

            var numberOfProductsInTheUsersCart = await this.cartsService.GetNumberOfProductsInTheUsersCart(message.UserId);

            await this.hubContext
                .Clients
                .User(context.Message.UserId)
                .SendAsync("NumberOfProductsInTheUsersCart", numberOfProductsInTheUsersCart);
        }
    }
}
