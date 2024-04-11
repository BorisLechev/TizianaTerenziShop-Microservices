namespace TizianaTerenzi.Carts.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Carts.Data.Models;
    using TizianaTerenzi.Carts.Services.Data.Carts;
    using TizianaTerenzi.Common.Data.Models;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Carts;
    using TizianaTerenzi.Common.Messages.Products;

    public class ProductAddedInTheCartConsumer : IConsumer<ProductAddedInTheCartMessage>
    {
        private readonly ICartsService cartsService;
        private readonly IBus publisher;
        private readonly IDeletableEntityRepository<Cart> cartsRepository;

        public ProductAddedInTheCartConsumer(
            ICartsService cartsService,
            IBus publisher,
            IDeletableEntityRepository<Cart> cartsRepository)
        {
            this.cartsService = cartsService;
            this.publisher = publisher;
            this.cartsRepository = cartsRepository;
        }

        public async Task Consume(ConsumeContext<ProductAddedInTheCartMessage> context)
        {
            var message = context.Message;

            var ifProductInTheCartExists = await this.cartsService.CheckIfProductExistsInTheUsersCartAsync(message.UserId, message.ProductId);

            if (ifProductInTheCartExists)
            {
                var productInTheCartId = await this.cartsService.GetProductInTheCartIdByProductIdAsync(message.ProductId, message.UserId);

                await this.cartsService.IncreaseQuantityAsync(productInTheCartId, message.UserId);
            }
            else
            {
                await this.cartsService.AddProductInTheCartAsync(message);
            }

            var numberOfProductsInTheUsersCart = await this.cartsService.GetNumberOfProductsInTheUsersCart(message.UserId);

            var messageData = new NotificationsUpdatedWhenProductAddedInTheCartMessage
            {
                UserId = message.UserId,
                NumberOfProductsInTheUsersCart = numberOfProductsInTheUsersCart,
            };

            var messageLog = new EventMessageLog(messageData);

            await this.cartsRepository.CreateEventMessageLog(messageLog);
            await this.cartsRepository.SaveChangesAsync();

            await this.publisher.Publish(messageData);

            await this.cartsRepository.MarkEventMessageLogAsPublished(messageLog.Id);
        }
    }
}
