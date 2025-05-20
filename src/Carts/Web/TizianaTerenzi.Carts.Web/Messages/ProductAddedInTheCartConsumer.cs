namespace TizianaTerenzi.Carts.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Carts.Data.Models;
    using TizianaTerenzi.Carts.Services.Data.Carts;
    using TizianaTerenzi.Common.Data.EventualConsistencyMessages;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Carts;
    using TizianaTerenzi.Common.Messages.Products;

    public class ProductAddedInTheCartConsumer : IConsumer<ProductAddedInTheCartMessage>
    {
        private readonly ICartsService cartsService;
        private readonly IDeletableEntityRepository<Cart> cartsRepository;
        private readonly IEventualConsistencyMessagesService eventualConsistencyMessagesService;

        public ProductAddedInTheCartConsumer(
            ICartsService cartsService,
            IDeletableEntityRepository<Cart> cartsRepository,
            IEventualConsistencyMessagesService eventualConsistencyMessagesService)
        {
            this.cartsService = cartsService;
            this.cartsRepository = cartsRepository;
            this.eventualConsistencyMessagesService = eventualConsistencyMessagesService;
        }

        public async Task Consume(ConsumeContext<ProductAddedInTheCartMessage> context)
        {
            var message = context.Message;

            var isDuplicated = await this.eventualConsistencyMessagesService.IsDuplicated(
                                                message,
                                                nameof(ProductAddedInTheCartMessage.CorrelationId),
                                                message.CorrelationId);

            if (isDuplicated)
            {
                return;
            }

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

            var eventMessage = new NotificationsUpdatedWhenProductAddedInTheCartMessage
            {
                UserId = message.UserId,
                NumberOfProductsInTheUsersCart = numberOfProductsInTheUsersCart,
            };

            await this.cartsRepository.SaveAndPublishEventMessageAsync(eventMessage);
        }
    }
}
