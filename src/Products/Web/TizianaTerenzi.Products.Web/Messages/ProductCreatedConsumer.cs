namespace TizianaTerenzi.Products.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Administration;
    using TizianaTerenzi.Products.Services.Cloudinary;
    using TizianaTerenzi.Products.Services.Data.Products;

    public class ProductCreatedConsumer : IConsumer<ProductCreatedMessage>
    {
        private readonly ICloudinaryService cloudinaryService;
        private readonly IProductsService productsService;

        public ProductCreatedConsumer(
            ICloudinaryService cloudinaryService,
            IProductsService productsService)
        {
            this.cloudinaryService = cloudinaryService;
            this.productsService = productsService;
        }

        public async Task Consume(ConsumeContext<ProductCreatedMessage> context)
        {
            var message = context.Message;

            string pictureUrl = await this.cloudinaryService.UploadPictureAsByteArrayAsync(message.Picture, message.Name);
            await this.productsService.CreateProductAsync(message, pictureUrl);

            await Task.CompletedTask;
        }
    }
}
