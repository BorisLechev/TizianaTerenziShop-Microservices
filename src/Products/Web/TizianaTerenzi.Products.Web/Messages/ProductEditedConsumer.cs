namespace TizianaTerenzi.Products.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Administration;
    using TizianaTerenzi.Products.Services.Cloudinary;
    using TizianaTerenzi.Products.Services.Data.Products;

    public class ProductEditedConsumer : IConsumer<ProductEditedMessage>
    {
        private readonly ICloudinaryService cloudinaryService;
        private readonly IProductsService productsService;

        public ProductEditedConsumer(
            ICloudinaryService cloudinaryService,
            IProductsService productsService)
        {
            this.cloudinaryService = cloudinaryService;
            this.productsService = productsService;
        }

        public async Task Consume(ConsumeContext<ProductEditedMessage> context)
        {
            var message = context.Message;

            string pictureUrl = string.Empty;

            if (message.Picture != null)
            {
                pictureUrl = await this.cloudinaryService.UploadPictureAsByteArrayAsync(message.Picture, message.Name);
            }

            await this.productsService.EditProductAsync(message, pictureUrl);

            await Task.CompletedTask;
        }
    }
}
