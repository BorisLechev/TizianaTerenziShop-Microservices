namespace TizianaTerenzi.Administration.Services.Data.Products
{
    using MassTransit;
    using TizianaTerenzi.Administration.Web.Models.Products;
    using TizianaTerenzi.Common.Messages.Administration;

    public class ProductsService : IProductsService
    {
        private readonly IBus publisher;

        public ProductsService(IBus publisher)
        {
            this.publisher = publisher;
        }

        public async Task CreateProductAsync(CreateProductInputModel inputModel, byte[] picture)
        {
            var notesIds = inputModel.NoteIds
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToArray();

            await this.publisher.Publish(new ProductCreatedMessage
            {
                Name = inputModel.Name,
                Description = inputModel.Description,
                FragranceGroupId = inputModel.FragranceGroupId,
                NoteIds = notesIds,
                Picture = picture,
                Price = inputModel.Price,
                ProductTypeId = inputModel.ProductTypeId,
                YearOfManufacture = inputModel.YearOfManufacture,
            });
        }

        public async Task EditProductAsync(EditProductInputModel inputModel, byte[]? picture)
        {
            var notesIds = inputModel.NoteIds
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToArray();

            await this.publisher.PublishBatch(new object[]
            {
                new ProductEditedMessage
                {
                    ProductId = inputModel.ProductId,
                    Name = inputModel.Name,
                    Description = inputModel.Description,
                    FragranceGroupId = inputModel.FragranceGroupId,
                    NoteIds = notesIds,
                    Picture = picture,
                    Price = inputModel.Price,
                    ProductTypeId = inputModel.ProductTypeId,
                    YearOfManufacture = inputModel.YearOfManufacture,
                },
                new ProductInTheCartsEditedMessage
                {
                    Name = inputModel.Name,
                    Price = inputModel.Price,
                    ProductId = inputModel.ProductId,
                },
            });
        }

        public async Task DeleteProductAsync(int productId)
        {
            await this.publisher.Publish(new ProductDeletedMessage
            {
                ProductId = productId,
            });
        }
    }
}
