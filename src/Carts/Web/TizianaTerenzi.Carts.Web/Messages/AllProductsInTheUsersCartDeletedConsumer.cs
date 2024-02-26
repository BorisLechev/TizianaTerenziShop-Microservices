namespace TizianaTerenzi.Carts.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Carts.Services.Data.Carts;
    using TizianaTerenzi.Common.Messages.Identity;

    public class AllProductsInTheUsersCartDeletedConsumer : IConsumer<AllProductsInTheUsersCartDeletedMessage>
    {
        private readonly ICartsService cartsService;

        public AllProductsInTheUsersCartDeletedConsumer(ICartsService cartsService)
        {
            this.cartsService = cartsService;
        }

        public async Task Consume(ConsumeContext<AllProductsInTheUsersCartDeletedMessage> context)
        {
            var message = context.Message;

            var result = await this.cartsService.DeleteAllProductsInTheCartByUserIdAsync(message.UserId);

            await Task.CompletedTask;
        }
    }
}
