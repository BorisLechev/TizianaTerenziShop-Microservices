namespace TizianaTerenzi.Carts.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Carts.Services.Data.Carts;
    using TizianaTerenzi.Common.Messages.Administration;

    public class ProductInAllCartsEditedConsumer : IConsumer<ProductInAllCartsEditedMessage>
    {
        private readonly ICartsService cartsService;

        public ProductInAllCartsEditedConsumer(ICartsService cartsService)
        {
            this.cartsService = cartsService;
        }

        public async Task Consume(ConsumeContext<ProductInAllCartsEditedMessage> context)
        {
            var message = context.Message;

            var result = await this.cartsService.EditProductInTheCartAsync(message);

            await Task.CompletedTask;
        }
    }
}
