namespace TizianaTerenzi.Products.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Identity;
    using TizianaTerenzi.Products.Services.Data.Wishlist;

    public class AllProductsInTheUsersWishlistDeletedConsumer : IConsumer<AllProductsInTheUsersWishlistDeletedMessage>
    {
        private readonly IWishlistService wishlistService;

        public AllProductsInTheUsersWishlistDeletedConsumer(IWishlistService wishlistService)
        {
            this.wishlistService = wishlistService;
        }

        public async Task Consume(ConsumeContext<AllProductsInTheUsersWishlistDeletedMessage> context)
        {
            var message = context.Message;

            var result = await this.wishlistService.DeleteAllProductsInTheWishlistAsync(message.UserId);

            await Task.CompletedTask;
        }
    }
}
