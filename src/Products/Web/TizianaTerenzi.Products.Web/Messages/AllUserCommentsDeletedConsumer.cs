namespace TizianaTerenzi.Products.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Identity;
    using TizianaTerenzi.Products.Services.Data.Comments;

    public class AllUserCommentsDeletedConsumer : IConsumer<AllUserCommentsDeletedMessage>
    {
        private readonly ICommentsService commentsService;

        public AllUserCommentsDeletedConsumer(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        public async Task Consume(ConsumeContext<AllUserCommentsDeletedMessage> context)
        {
            var message = context.Message;

            var result = await this.commentsService.DeleteRangeByUserIdAsync(message.UserId);

            await Task.CompletedTask;
        }
    }
}
