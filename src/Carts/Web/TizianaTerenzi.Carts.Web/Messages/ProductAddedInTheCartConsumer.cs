namespace TizianaTerenzi.Carts.Web.Messages
{
    using MassTransit;
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
                var productInTheCartId = await this.cartsService.GetProductInTheCartIdByProductIdAsync(message.ProductId);

                await this.cartsService.IncreaseQuantityAsync(productInTheCartId);
            }
            else
            {
                await this.cartsService.AddProductInTheCartAsync(message);
            }
        }
    }
}
