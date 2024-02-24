namespace TizianaTerenzi.Carts.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Carts.Services.Data.Carts;
    using TizianaTerenzi.Common.Messages.Administration;

    public class ProductInTheCartsEditedConsumer : IConsumer<ProductInTheCartsEditedMessage>
    {
        private readonly ICartsService cartsService;

        public ProductInTheCartsEditedConsumer(ICartsService cartsService)
        {
            this.cartsService = cartsService;
        }

        public async Task Consume(ConsumeContext<ProductInTheCartsEditedMessage> context)
        {
            var message = context.Message;

            var result = await this.cartsService.EditProductInTheCartAsync(message);

            await Task.CompletedTask;
        }
    }
}
