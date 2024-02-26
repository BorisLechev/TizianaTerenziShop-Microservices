namespace TizianaTerenzi.Products.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Identity;
    using TizianaTerenzi.Products.Services.Data.Votes;

    public class AllUserCommentVotesDeletedConsumer : IConsumer<AllUserCommentVotesDeletedMessage>
    {
        private readonly ICommentVotesService commentVotesService;

        public AllUserCommentVotesDeletedConsumer(ICommentVotesService commentVotesService)
        {
            this.commentVotesService = commentVotesService;
        }

        public async Task Consume(ConsumeContext<AllUserCommentVotesDeletedMessage> context)
        {
            var message = context.Message;

            var result = await this.commentVotesService.DeleteRangeByUserIdAsync(message.UserId);

            await Task.CompletedTask;
        }
    }
}
