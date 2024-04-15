namespace TizianaTerenzi.Administration.Services.Data.Products
{
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Administration.Web.Models.Products;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Administration;

    public class ProductsService : IProductsService
    {
        private readonly IDeletableEntityRepository<OrderProductStatistics> repository;

        public ProductsService(
            IDeletableEntityRepository<OrderProductStatistics> repository)
        {
            this.repository = repository;
        }

        public async Task CreateProductAsync(CreateProductInputModel inputModel, byte[] picture)
        {
            var notesIds = inputModel.NoteIds
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToArray();

            var message = new ProductCreatedMessage
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

            await this.repository.SaveAndPublishEventMessageAsync(message);
        }

        public async Task EditProductAsync(EditProductInputModel inputModel, byte[]? picture)
        {
            var notesIds = inputModel.NoteIds
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToArray();

            var messageProductEdited = new ProductEditedMessage
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

            var messageProductInAllCartsEdited = new ProductInAllCartsEditedMessage
            {
                Name = inputModel.Name,
                Price = inputModel.Price,
                ProductId = inputModel.ProductId,
            };

            await this.repository.SaveAndPublishEventMessageAsync(messageProductEdited, messageProductInAllCartsEdited);
        }

        public async Task DeleteProductAsync(int productId)
        {
            var messageProductDelete = new ProductDeletedMessage
            {
                ProductId = productId,
            };

            var messageProductInUserCartsDeleted = new ProductInAllCartsDeletedMessage
            {
                ProductId = productId,
            };

            await this.repository.SaveAndPublishEventMessageAsync(messageProductDelete, messageProductInUserCartsDeleted);
        }
    }
}
