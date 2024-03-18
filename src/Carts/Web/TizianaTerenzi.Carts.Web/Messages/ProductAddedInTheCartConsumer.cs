namespace TizianaTerenzi.Carts.Web.Messages
{
    using MassTransit;
    using Microsoft.AspNetCore.SignalR.Client;
    using TizianaTerenzi.Carts.Services.Data.Carts;
    using TizianaTerenzi.Common.Messages.Products;

    public class ProductAddedInTheCartConsumer : IConsumer<ProductAddedInTheCartMessage>
    {
        private readonly ICartsService cartsService;

        public ProductAddedInTheCartConsumer(ICartsService cartsService)
        {
            this.cartsService = cartsService;
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

            HubConnection connection = new HubConnectionBuilder()
                                        .WithUrl("https://localhost:5011/numberOfProductsInTheUsersCartHub")
                                        .Build();

            await connection.StartAsync();

            await connection.InvokeAsync("AddProductInTheUsersCart", numberOfProductsInTheUsersCart, message.UserId);

            await connection.StopAsync();
        }
    }
}
