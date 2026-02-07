namespace TizianaTerenzi.Products.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Administration;
    using TizianaTerenzi.Products.Services.Data.Comments;
    using TizianaTerenzi.Products.Services.Data.Notes;
    using TizianaTerenzi.Products.Services.Data.Products;
    using TizianaTerenzi.Products.Services.Data.Votes;

    public class ProductDeletedConsumer : IConsumer<ProductDeletedMessage>
    {
        private readonly INotesService notesService;
        private readonly ICommentsService commentsService;
        private readonly ICommentVotesService commentVotesService;
        private readonly IProductVotesService productVotesService;
        private readonly IProductsService productsService;

        public ProductDeletedConsumer(
            INotesService notesService,
            ICommentsService commentsService,
            ICommentVotesService commentVotesService,
            IProductVotesService productVotesService,
            IProductsService productsService)
        {
            this.notesService = notesService;
            this.commentsService = commentsService;
            this.commentVotesService = commentVotesService;
            this.productVotesService = productVotesService;
            this.productsService = productsService;
        }

        public async Task Consume(ConsumeContext<ProductDeletedMessage> context)
        {
            var message = context.Message;

            await this.notesService.SoftDeleteAllProductNotesAsync(message.ProductId);
            await this.commentVotesService.DeleteRangeByProductIdAsync(message.ProductId);
            await this.commentsService.DeleteRangeByProductIdAsync(message.ProductId);
            await this.productVotesService.DeleteProductVotesAsync(message.ProductId);
            await this.productsService.SoftDeleteProductAsync(message.ProductId);

            await Task.CompletedTask;
        }
    }
}
