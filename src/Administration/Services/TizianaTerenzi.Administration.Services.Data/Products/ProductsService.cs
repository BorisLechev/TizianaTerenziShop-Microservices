namespace TizianaTerenzi.Administration.Services.Data.Products
{
    using MassTransit;
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Administration.Web.Models.Products;
    using TizianaTerenzi.Common.Data.Models;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Administration;

    public class ProductsService : IProductsService
    {
        private readonly IBus publisher;
        private readonly IDeletableEntityRepository<OrderProductStatistics> repository;

        public ProductsService(
            IBus publisher,
            IDeletableEntityRepository<OrderProductStatistics> repository)
        {
            this.publisher = publisher;
            this.repository = repository;
        }

        public async Task CreateProductAsync(CreateProductInputModel inputModel, byte[] picture)
        {
            var notesIds = inputModel.NoteIds
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToArray();

            var messageData = new ProductCreatedMessage
            {
                Name = inputModel.Name,
                Description = inputModel.Description,
                FragranceGroupId = inputModel.FragranceGroupId,
                NoteIds = notesIds,
                Picture = picture,
                Price = inputModel.Price,
                ProductTypeId = inputModel.ProductTypeId,
                YearOfManufacture = inputModel.YearOfManufacture,
            };

            var message = new EventMessageLog(messageData);

            await this.repository.CreateEventMessageLog(message);
            await this.repository.SaveChangesAsync();

            await this.publisher.Publish(messageData);

            await this.repository.MarkEventMessageLogAsPublished(message.Id);
        }

        public async Task EditProductAsync(EditProductInputModel inputModel, byte[]? picture)
        {
            var notesIds = inputModel.NoteIds
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToArray();

            var messageDataProductEdited = new ProductEditedMessage
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
            };

            var messageDataProductInAllCartsEdited = new ProductInAllCartsEditedMessage
            {
                Name = inputModel.Name,
                Price = inputModel.Price,
                ProductId = inputModel.ProductId,
            };

            var messageProductEdited = new EventMessageLog(messageDataProductEdited);
            var messageProductInAllCartsEdited = new EventMessageLog(messageDataProductInAllCartsEdited);

            await this.repository.CreateEventMessageLog(messageProductEdited, messageProductInAllCartsEdited);
            await this.repository.SaveChangesAsync();

            await this.publisher.PublishBatch(new object[]
            {
                messageDataProductEdited,
                messageDataProductInAllCartsEdited,
            });

            await this.repository.MarkEventMessageLogAsPublished(messageProductEdited.Id, messageProductInAllCartsEdited.Id);
        }

        public async Task DeleteProductAsync(int productId)
        {
            var messageDataProductDelete = new ProductDeletedMessage
            {
                ProductId = productId,
            };

            var messageDataProductInUserCartsDeleted = new ProductInAllCartsDeletedMessage
            {
                ProductId = productId,
            };

            var messageProductDelete = new EventMessageLog(messageDataProductDelete);
            var messageProductInUserCartsDeleted = new EventMessageLog(messageDataProductInUserCartsDeleted);

            await this.repository.CreateEventMessageLog(messageProductDelete, messageProductInUserCartsDeleted);

            await this.repository.SaveChangesAsync();

            await this.publisher.PublishBatch(new object[]
            {
                messageDataProductDelete,
                messageDataProductInUserCartsDeleted,
            });

            await this.repository.MarkEventMessageLogAsPublished(messageProductDelete.Id, messageProductInUserCartsDeleted.Id);
        }
    }
}
