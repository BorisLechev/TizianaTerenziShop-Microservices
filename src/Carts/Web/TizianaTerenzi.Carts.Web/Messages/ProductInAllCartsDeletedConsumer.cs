namespace TizianaTerenzi.Carts.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Carts.Services.Data.Carts;
    using TizianaTerenzi.Common.Messages.Administration;

    public class ProductInAllCartsDeletedConsumer : IConsumer<ProductInAllCartsDeletedMessage>
    {
        private readonly ICartsService cartsService;

        public ProductInAllCartsDeletedConsumer(ICartsService cartsService)
        {
            this.cartsService = cartsService;
        }

        public async Task Consume(ConsumeContext<ProductInAllCartsDeletedMessage> context)
        {
            var message = context.Message;

            var result = await this.cartsService.DeleteProductInAllCartsAsync(message);

            await Task.CompletedTask;
        }
    }
}
